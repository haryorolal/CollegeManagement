using AutoMapper;
using Azure;
using CollegeManagement.Data.Identity;
using CollegeManagement.Data.IRepository;
using CollegeManagement.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Net;
using CollegeManagement.Data.IServices;

namespace CollegeManagement.Data.Repository
{
    public class UserService : CollegeRepository<User>, IUserService
    {
        private readonly LibraryDbContext _libraryDbContext;
        private readonly IMapper _mapper;
        private APIResponse _apiResponse;
        public UserService(LibraryDbContext libraryDbContext, IMapper mapper) : base(libraryDbContext, mapper)
        {
            _libraryDbContext = libraryDbContext;
            _mapper = mapper;
            _apiResponse = new APIResponse();
        }

        public async Task<APIResponse> AuthenticateUser(LoginDTO dto, IConfiguration _configuration)
        {
            LoginResponseDTO response = new LoginResponseDTO();    
            
            //find user
            User user = await GetAsync(x => x.Username == dto.Username);
            if (user == null)
                return _apiResponse.ResponseToClient(false, HttpStatusCode.NotFound, "Invalid Login Credentials", null, string.Empty);

            //validate password
            var inputHashPassword = HashPasswordWithExistingSalt(dto.Password, user.PasswordSalt);
            if (inputHashPassword != user.Password)
                return _apiResponse.ResponseToClient(false, HttpStatusCode.NotFound, "Invalid Login Credentials", null, string.Empty);

            //load userType (SuperAdmin, Admin, Staff, Student)
            var userType = await _libraryDbContext.UserTypes.SingleOrDefaultAsync(ut => ut.Id == user.UserTypeId);
            if (userType == null)
                return _apiResponse.ResponseToClient(false, HttpStatusCode.NotFound, "Invalid User Type", null, string.Empty);

            // map usertType to role
            var userRole = await _libraryDbContext.Roles.SingleOrDefaultAsync(x => x.RoleName == userType.Name);
            if (userRole == null)
                return _apiResponse.ResponseToClient(false, HttpStatusCode.NotFound, "No role found for user", null, string.Empty);

            SchoolUser? schoolUser = null;
            int? schoolId = null;

            // check school association (except superAdmin)
            if (!string.Equals(userType.Code, "SADMIN", StringComparison.OrdinalIgnoreCase))
            {
                if (userType.Code == "SDT") //student
                {
                    schoolId = await _libraryDbContext.Students.Where(s => s.UserId == user.Id).Select(s => s.SchoolId).FirstOrDefaultAsync();
                }
                else if (userType.Code == "STF") //staffs
                {
                    schoolId = await _libraryDbContext.Staffs.Where(s => s.UserId == user.Id).Select(s => s.SchoolId).FirstOrDefaultAsync();
                }
                else if (userType.Code == "ADMIN") //adminn
                {
                    schoolId = await _libraryDbContext.SchoolAdmins.Where(s => s.UserId == user.Id).Select(s => s.SchoolId).FirstOrDefaultAsync();
                }
                else if (userType.Code == "SDP") //student parent
                {
                    schoolId = await _libraryDbContext.Parents.Where(s => s.UserId == user.Id).Select(s => s.SchoolId).FirstOrDefaultAsync();
                }

                if (schoolId.HasValue)
                {
                    schoolUser = await _libraryDbContext.SchoolUsers.FirstOrDefaultAsync(ur => ur.UserId == user.Id && ur.Role == userRole.RoleName && ur.SchoolId == schoolId.Value);

                    //autho-add mapping if missing
                    if (schoolUser == null)
                        await AddUserToRole(user.Id, userRole.RoleName, schoolId.Value);
                }               
            }

            // Build claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, dto.Username),
                new Claim(ClaimTypes.Role, userRole.RoleName),
                new Claim("UserId", user.Id.ToString()),
                new Claim("UserType", userType.Name)
            };

            if (schoolId.HasValue)
                claims.Add(new Claim("SchoolId", schoolId.Value.ToString()));

            // Generate JWT token
            var key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("JWTSecretKey"));
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(4),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenGenerated = tokenHandler.WriteToken(token);

            // Build Response
            response.Username = dto.Username;
            response.Token = tokenGenerated;
            response.Role = userRole.RoleName;
            //response.RoleId = userRole.Id;
            //response.SchoolId = userRole.RoleName != "SuperAdmin" ?  schoolId.Value : 0; //causing issue, please look on it tomorrow
            response.SchoolUser = userRole.RoleName != "SuperAdmin" ? new SchoolUser
            {
                Id = schoolUser.Id,
                Role = schoolUser.Role,
                UserId = schoolUser.UserId,
                SchoolId = schoolUser.SchoolId
            } : new SchoolUser();
            response.UserTypeId = userType.Id;
            response.IsActive = true;

            return _apiResponse.ResponseToClient(true, System.Net.HttpStatusCode.OK, "Successful Login", response, string.Empty);                         
        }

        public async Task<List<UserResponse>> GetAllUsers()
        {
            var results = await GetAllFilterAsync(u => !u.IsDeleted, null, new List<string> { "SchoolUsers", "Students", "Staffs", "SchoolAdmins", "UserType" }, false);
            if (results == null)
                throw new Exception("Could not get any user datas");

            var userMapper = _mapper.Map<List<UserResponse>>(results);

            return userMapper;
        }

        //public async Task<List<UserResponse>> GetAllAdminUsers()
        //{
        //    //var getRole = await _libraryDbContext.
        //    var results = await GetAllFilterAsync(u => !u.IsDeleted, null, new List<string> { "SchoolUser" }, false );
        //    if (results == null)
        //        throw new Exception("Could not get any user datas");

        //    var userMapper = _mapper.Map<List<UserResponse>>(results);

        //    return userMapper;
        //}
        public async Task<UserResponse> GetUserById(int Id)
        {
            if (Id <= 0)
                throw new Exception("The user id is not valid");    

            var result = await GetAsync(x => !x.IsDeleted && x.Id == Id, new List<string> { "SchoolUsers", "Students", "Staffs", "SchoolAdmins", "UserType" });
            if (result == null)
                throw new Exception("Could not find the user data");

            var userMapper = _mapper.Map<UserResponse>(result);
            return userMapper;
        }
        public async Task<UserResponse> GetUserByUsername(string Username)
        {
            if (Username == null)
                throw new Exception("The username is not valid");

            var result = await GetAsync(x => !x.IsDeleted && x.Username.Equals(Username), new List<string> { "SchoolUsers", "Students", "Staffs", "SchoolAdmins", "UserType" });
            if (result == null)
                throw new Exception("Could not find the user data");

            var userMapper = _mapper.Map<UserResponse>(result);
            return userMapper;
        }
        public async Task<bool> CreateUserAsync(UserDTO dto)
        {
            ArgumentNullException.ThrowIfNull(dto, $"The argument {nameof(dto)} is null");

            var existingUser = await GetAsync(x => !x.IsDeleted && x.Username.Equals(dto.Username));
            if (existingUser != null)
                throw new Exception("The username already taken");

            User user = _mapper.Map<User>(dto);
            user.IsDeleted = false;
            user.CreatedDate = DateTime.Now;
            user.ModifiedDate = DateTime.Now;

            if (!string.IsNullOrEmpty(dto.Password))
            {
                var passwordHash = CreatePasswordHashWithSalt(dto.Password);
                user.Password = passwordHash.PasswordHash;
                user.PasswordSalt = passwordHash.Salt;
            }

            await CreateAsync(user);

            //load userType (SuperAdmin, Admin, Staff, Student)
            var userType = await _libraryDbContext.UserTypes.FirstOrDefaultAsync(ut => ut.Id == user.UserTypeId)
                                ?? throw new Exception($"user type not found");
            // map usertType to role
            var userRole = await _libraryDbContext.Roles.FirstOrDefaultAsync(x => x.RoleName == userType.Name)
                                ?? throw new Exception($"user role not found");

            await AddUserToRole(user.Id, userRole.RoleName, dto.schoolId.Value);

            return true;

        }

        public async Task<bool> UpdateUser(UserDTO dto)
        {
            ArgumentNullException.ThrowIfNull(dto, $"The argument {nameof(dto)} is null");

            // Get the user WITH tracking this time
            var existingUser = await GetAsync(x => !x.IsDeleted && x.Id == dto.Id, null, true);
            if (existingUser == null)
                throw new Exception("The user does not exist");

            // Update properties directly on the tracked entity
            //var updatedUser = _mapper.Map<User>(dto);
            existingUser.Username = dto.Username;
            existingUser.UserTypeId = dto.UserTypeId;
            existingUser.IsActive = dto.IsActive;
            existingUser.ModifiedDate = DateTime.Now;

            if (!string.IsNullOrEmpty(dto.Password))
            {
                var passwordHash = CreatePasswordHashWithSalt(dto.Password);
                existingUser.Password = passwordHash.PasswordHash;
                existingUser.PasswordSalt = passwordHash.Salt;
            }

            // No need to call UpdateAsync since the entity is tracked
            //await _libraryDbContext.SaveChangesAsync();
            await UpdateAsync(existingUser);

            //load userType (SuperAdmin, Admin, Staff, Student)
            var userType = await _libraryDbContext.UserTypes.FirstOrDefaultAsync(ut => ut.Id == existingUser.UserTypeId)
                                ?? throw new Exception($"user type not found");

            await AddUserToRole(existingUser.Id, userType.Name, dto.schoolId.Value);

            return true;

        }
        private async Task<SchoolUser> AddUserToRole(int userId, string roleName, int schoolId)
        {
            var existingUser = await GetAsync(x => !x.IsDeleted && x.Id == userId);
            if (existingUser == null)
                throw new Exception("The user does not exist");

            var existingRole = await _libraryDbContext.Roles.FirstOrDefaultAsync(r => r.RoleName == roleName);
            if (existingRole == null)
                throw new Exception("The role does not exist");

            SchoolUser schoolUser = await _libraryDbContext.SchoolUsers.FirstOrDefaultAsync(ur => ur.UserId == userId && ur.SchoolId == schoolId);
            if (schoolUser == null)
            {
                schoolUser = new SchoolUser
                {
                    UserId = userId,
                    SchoolId = schoolId,
                    Role = roleName,
                };                

                await _libraryDbContext.SchoolUsers.AddAsync(schoolUser);
            }else
            {
                schoolUser.UserId = userId;
                schoolUser.SchoolId = schoolId;
                schoolUser.Role = roleName;

                _libraryDbContext.SchoolUsers.Update(schoolUser);
            }

            await _libraryDbContext.SaveChangesAsync();
            return schoolUser;
        }

        public async Task<bool> DeleteUser(int Id)
        {
            if (Id <= 0)
                throw new Exception("The Id does not exist");

            var existingUser = await GetAsync(x => !x.IsDeleted && x.Id == Id, null, true);
            if (existingUser == null)
                throw new Exception("The username does not exist");

            //soft delete
            existingUser.IsActive = false;
            existingUser.IsDeleted = true;            
            await UpdateAsync(existingUser); 
            // soft delete ends

            //await DeleteAsync(existingUser); hard Delete
            return true;

        }
        private (string PasswordHash, string Salt) CreatePasswordHashWithSalt(string passwordd)
        {
            //create the salt
            var salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            //create password hash
            var hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: passwordd,
                salt = salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
             ));

            return (hash, Convert.ToBase64String(salt));
        }
        private string HashPasswordWithExistingSalt(string password, string base64Salt)
        {
            var salt = Convert.FromBase64String(base64Salt);

            var hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
            ));

            return hash;
        }

    }
}
