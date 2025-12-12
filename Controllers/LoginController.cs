using CollegeManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using CollegeManagement.Data.IRepository;
using System.Threading.Tasks;
using CollegeManagement.Data.Identity;
using System.Net;

namespace CollegeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class LoginController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICollegeRepository<Role> _roleRepository;
        private readonly IConfiguration _configuration;
        private APIResponse _apiResponse;
        public LoginController(IConfiguration configuration, IUnitOfWork unitOfWork, ICollegeRepository<Role> collegeRepository)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _roleRepository = collegeRepository;
            _apiResponse = new APIResponse();
        }

        [HttpPost]
        public async Task<ActionResult<APIResponse>> Login(LoginDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Please provide username and password");
                /*
                LoginResponseDTO response = new LoginResponseDTO();
                var user = await _unitOfWork.UserServiceInterface.GetAllFilterAsync(x => x.Username == dto.Username && x.Password == dto.Password);
                var role = await _roleRepository.GetAllFilterAsync(r => r.RoleName == "Admin");

                if (user != null)
                {
                    var key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("JWTSecretKey"));
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var tokenDescriptor = new SecurityTokenDescriptor()
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.Name, dto.Username),
                            new Claim(ClaimTypes.Role, "Admin")
                        }),
                        Expires = DateTime.Now.AddHours(4),
                        SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    var tokenGenerated = tokenHandler.WriteToken(token);
                    response.Username = dto.Username;
                    response.Token = tokenGenerated;
                }
                else
                {
                    return BadRequest("Invalid username and password");
                }
                */

                APIResponse response = await _unitOfWork.UserServiceInterface.AuthenticateUser(dto, _configuration);

                return Ok(response);
            }catch(Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Something went wrong", null, ex.Message);
            }
        }
    }
}
