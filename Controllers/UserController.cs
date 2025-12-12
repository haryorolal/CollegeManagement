using AutoMapper;
using CollegeManagement.Data.Identity;
using CollegeManagement.Data.IRepository;
using CollegeManagement.Data.IServices;
using CollegeManagement.Data.Model;
using CollegeManagement.Data.Repository;
using CollegeManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CollegeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private APIResponse _apiResponse;
        private readonly IUploadExcel<UserDTO> _uploadExcel;
        private readonly LibraryDbContext _dbContext;
        public UserController(IUnitOfWork unitOfWork, ILogger<UserController> logger, IMapper mapper, IUploadExcel<UserDTO> uploadExcel, LibraryDbContext libraryDbContext)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _apiResponse = new APIResponse();
            _logger = logger;
            _uploadExcel = uploadExcel;
            _dbContext = libraryDbContext;
        }

        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetAllUsers()
        {
            try
            {
                _logger.LogInformation("Getting all users");
                var allUsers = await _unitOfWork.UserServiceInterface.GetAllUsers();
                if (allUsers == null)
                {
                   _logger.LogError($"Could not get users: {allUsers}");
                   return _apiResponse.ResponseToClient(false, HttpStatusCode.NotFound, "Could not get users", null, string.Empty);
                }
                    

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "User retrieved successfully", allUsers, string.Empty);
                _logger.LogInformation($"Successfully got all users");
                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Something went wrong", null, ex.Message);
            }
        }

        [HttpGet("allstudents")]
        public async Task<ActionResult<APIResponse>> GetAllStudents()
        {
            try
            {
                _logger.LogInformation("Getting all students");
                var allUsers = await _unitOfWork.UserServiceInterface.GetAllFilterAsync(x => x.UserType.Code == "SDT");
                if (allUsers == null || !allUsers.Any())
                {
                    _logger.LogError($"Could not get users: {allUsers}");
                    return _apiResponse.ResponseToClient(false, HttpStatusCode.NotFound, "Could not get users", null, string.Empty);
                }


                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "User retrieved successfully", allUsers, string.Empty);
                _logger.LogInformation($"Successfully got all users");
                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Something went wrong", null, ex.Message);
            }
        }

        [HttpGet("allstaffs")]
        public async Task<ActionResult<APIResponse>> GetAllStaffs()
        {
            try
            {
                _logger.LogInformation("Getting all staffs");
                var allUsers = await _unitOfWork.UserServiceInterface.GetAllFilterAsync(x => x.UserType.Code == "STF");
                if (allUsers == null || !allUsers.Any())
                {
                    _logger.LogError($"Could not get users: {allUsers}");
                    return _apiResponse.ResponseToClient(false, HttpStatusCode.NotFound, "Could not get users", null, string.Empty);
                }


                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "User retrieved successfully", allUsers, string.Empty);
                _logger.LogInformation($"Successfully got all users");
                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Something went wrong", null, ex.Message);
            }
        }

        [HttpGet("usersBySchool/students/{schoolId}")]
        public async Task<ActionResult<APIResponse>> GetAllStudentUsersBySchoolAdmin(int schoolId)
        {
            try
            {
                _logger.LogInformation($"Getting all student users by school - {schoolId} in GetAllStudentUsersBySchoolAdmin");
                List<User> allUsers = new List<User>();

                var userType = _dbContext.UserTypes.FirstOrDefault(x => x.Code == "SDT");

                allUsers = await _unitOfWork.UserServiceInterface.GetAllFilterAsync(x => x.SchoolUsers.Any(x => x.SchoolId == schoolId) && x.UserTypeId == userType.Id  );
                if (allUsers == null || !allUsers.Any())
                {
                    _logger.LogError($"Could not get users by admin: {allUsers}");
                    return _apiResponse.ResponseToClient(false, HttpStatusCode.NotFound, "Could not get student users", null, string.Empty);
                }


                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "User retrieved successfully", allUsers, string.Empty);
                _logger.LogInformation($"Successfully got all users by Admin");
                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Something went wrong", null, ex.Message);
            }
        }

        [HttpGet("usersBySchool/staffs/{schoolId}")]
        public async Task<ActionResult<APIResponse>> GetAllStaffUsersBySchoolAdmin(int schoolId)
        {
            try
            {
                _logger.LogInformation($"Getting all staff users by school - {schoolId} in GetAllStaffUsersBySchoolAdmin");
                List<User> allUsers = new List<User>();

                var userType = _dbContext.UserTypes.FirstOrDefault(x => x.Code == "STF");

                allUsers = await _unitOfWork.UserServiceInterface.GetAllFilterAsync(x => x.SchoolUsers.Any(x => x.SchoolId == schoolId) && x.UserTypeId == userType.Id);
                if (allUsers == null || !allUsers.Any())
                {
                    _logger.LogError($"Could not get users by admin: {allUsers}");
                    return _apiResponse.ResponseToClient(false, HttpStatusCode.NotFound, "Could not get staff users", null, string.Empty);
                }


                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "User retrieved successfully", allUsers, string.Empty);
                _logger.LogInformation($"Successfully got all users by Admin");
                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Something went wrong", null, ex.Message);
            }
        }


        [HttpGet("GetUserById/{Id}")]
        public async Task<ActionResult<APIResponse>> GetUserById(int Id)
        {
            try
            {
                _logger.LogInformation($"Getting user by Id: {Id}");
                var userResult = await _unitOfWork.UserServiceInterface.GetUserById(Id);
                if (userResult == null)
                    return _apiResponse.ResponseToClient(false, HttpStatusCode.NotFound, "Could not get user", null, string.Empty);

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "User retrieved successfully", userResult, string.Empty);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Something went wrong", null, ex.Message);
            }
        }

        [HttpGet("GetUserByName/{Username}")]
        public async Task<ActionResult<APIResponse>> GetUserByName(string Username)
        {
            try
            {
                _logger.LogInformation($"Getting user by Username: {Username}");
                var userResult = await _unitOfWork.UserServiceInterface.GetUserByUsername(Username);
                if (userResult == null)
                    return _apiResponse.ResponseToClient(false, HttpStatusCode.NotFound, "Could not get user", null, string.Empty);

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "User retrieved successfully", userResult, string.Empty);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Something went wrong", null, ex.Message);
            }
        }

        [HttpGet("search/{SearchBy}/{SearchText}")]
        public async Task<ActionResult<APIResponse>> SearchUser(string SearchBy, string SearchText)
        {
            if (SearchBy == null || SearchText == null)
                return _apiResponse.ResponseToClient(false, HttpStatusCode.BadRequest, "Fields cannot be empty", null, string.Empty);

            List<User> result = null;
            string[] searchItems = new[] { "Username" };            
            // Capitalize first letter to match property names
            SearchBy = char.ToUpper(SearchBy[0]) + SearchBy.Substring(1);

            for (var i = 0; i < searchItems.Length; i++)
            {
                if (SearchBy == searchItems[i])
                    result = await _unitOfWork.UserServiceInterface.GetAllFilterAsync(x => EF.Property<string>(x, SearchBy).ToLower().Contains(SearchText));
            }

            if (result == null)
                return _apiResponse.ResponseToClient(false, HttpStatusCode.NotFound, "Could not find any data related", null, string.Empty);

            var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully retrieved related data", result, string.Empty);
            return Ok(response);
        }

        [HttpPost("upload-excel")]
        public async Task<ActionResult<APIResponse>> UploadUsers(IFormFile file)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var userDto = await _uploadExcel.ImportExcelAsync(file);
                if (userDto == null || userDto.Count == 0)
                    return _apiResponse.ResponseToClient(false, HttpStatusCode.BadRequest, "No data found in the file", null, string.Empty);

                List<User> user = _mapper.Map<List<User>>(userDto);
                if (user is null) {
                    await _unitOfWork.RollbackAsync();
                    return _apiResponse.ResponseToClient(false, HttpStatusCode.BadRequest, "Could not convert data", null, string.Empty);
                }

                await _unitOfWork.UserServiceInterface.CreateRangeAsync(user);
                _unitOfWork.Save();
                await _unitOfWork.CommitAsync();

                var result = _apiResponse.ResponseToClient(true, HttpStatusCode.Created, "Successfully added all datas", null, string.Empty);
                return Ok(result);
            }
            catch(Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error adding all datas", null, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<APIResponse>> CreateUser([FromBody] UserDTO dto)
        {
            try
            {
                _logger.LogInformation($"Creating a new user {dto.Username}");
                if (dto == null)
                    return _apiResponse.ResponseToClient(false, HttpStatusCode.BadRequest, "Invalid user data", null, string.Empty);

                var userResult = await _unitOfWork.UserServiceInterface.CreateUserAsync(dto);
                if (userResult == false)
                    return _apiResponse.ResponseToClient(false, HttpStatusCode.NotFound, "Could not create user", null, string.Empty);

                _unitOfWork.Save();
                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.Created, "Successfully Created User", null, string.Empty);
                return CreatedAtAction("GetUserById", new {Id = dto.Id}, response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating user with Id: {dto.Username}");
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Something just happened", null, ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<APIResponse>> UpdateUser([FromBody] UserDTO dto)
        {
            try
            {
                _logger.LogInformation($"Updating user with username: {dto.Username}");
                if (dto == null)
                    return _apiResponse.ResponseToClient(false, HttpStatusCode.BadRequest, "Invalid user data", null, string.Empty);

                var userResult = await _unitOfWork.UserServiceInterface.UpdateUser(dto);
                if (userResult == false)
                    return _apiResponse.ResponseToClient(false, HttpStatusCode.NotFound, "Could not update user", null, string.Empty);

                _unitOfWork.Save();

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "User updated successfully", null, string.Empty);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating user with username: {dto.Username}");
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Something went wrong", null, ex.Message);
            }
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult<APIResponse>> DeleteUser(int Id)
        {
            try
            {
                _logger.LogInformation($"Deleting user with Id: {Id}");
                if (Id <= 0)
                    return _apiResponse.ResponseToClient(false, HttpStatusCode.BadRequest, "Invalid user data", null, string.Empty);

                var userResult = await _unitOfWork.UserServiceInterface.DeleteUser(Id);
                if (userResult == false)
                    return _apiResponse.ResponseToClient(false, HttpStatusCode.NotFound, "Could not delete user", null, string.Empty);

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "User deleted successfully", userResult, string.Empty);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting user with Id: {Id}");
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Something went wrong", null, ex.Message);
            }
        }




    }
}
