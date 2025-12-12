using CollegeManagement.Data;
using CollegeManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using CollegeManagement.Data.IRepository;
using CollegeManagement.Data.Identity;
using CollegeManagement.Data.IServices;
using Microsoft.AspNetCore.Authorization;
using CollegeManagement.Data.HelperMethod;
using CollegeManagement.Data.Services;

namespace CollegeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class StaffController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly APIResponse _apiResponse;
        private readonly LibraryDbContext _libraryDbContext;
        private readonly IEmailSenderService _emailSenderService;
        private readonly IUploadExcel<Staff> _uploadExcel;
        public StaffController(IUnitOfWork unitOfWork, IMapper mapper, LibraryDbContext libraryDbContext, IEmailSenderService emailSenderService, IUploadExcel<Staff> uploadExcel)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _apiResponse = new APIResponse();
            _libraryDbContext = libraryDbContext;
            _emailSenderService = emailSenderService;
            _uploadExcel = uploadExcel;
        }

        [HttpGet("generate")]
        public async Task<ActionResult<string>> GenerateStaffNumber()
        {
            var generatedStaffNumber = await _unitOfWork.StaffRepositoryInterface.GenerateStaffNumber();
            return Ok(new { staffNumber = generatedStaffNumber });
        }

        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetAllStaff()
        {
            try
            {
                var result = await _unitOfWork.StaffRepositoryInterface.GetAllAsync(new List<string> { "Department" });
                if (result == null)
                    return NotFound("Looks like there's no data yet");

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully retrieved data", result, string.Empty);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error retrieving data", null, ex.Message);
            }
        }

        [HttpGet("school/{schoolId}")]
        public async Task<ActionResult<APIResponse>> GetAllStaffBySchool(int schoolId)
        {
            try
            {
                var result = await _unitOfWork.StaffRepositoryInterface.GetAllFilterAsync(x => x.SchoolId == schoolId, null, new List<string> { "Department", "User", "School" });
                if (result == null)
                    return NotFound("Looks like there's no data yet");

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully retrieved data", result, string.Empty);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error retrieving data", null, ex.Message);
            }
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<APIResponse>> GetStaffById(int Id)
        {
            try
            {
                if (Id == 0)
                    return BadRequest("Id cannot be less than or equal to zero");

                var result = await _unitOfWork.StaffRepositoryInterface.GetAsync(f => f.Id == Id, new List<string> { "Department"}, false);
                if (result == null)
                    return NotFound("Looks like there's no data yet");

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully retrieved data", result, string.Empty);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error retrieving data", null, ex.Message);
            }
        }

        [HttpPost("upload-excel")]
        public async Task<ActionResult<APIResponse>> UploadStaffs(IFormFile file)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var itemDto = await _uploadExcel.ImportExcelAsync(file);
                if (itemDto == null || itemDto.Count == 0)
                    return _apiResponse.ResponseToClient(false, HttpStatusCode.BadRequest, "No data found in the file", null, string.Empty);

                var staffs = _mapper.Map<List<Staff>>(itemDto);
                if (staffs == null)
                    return _apiResponse.ResponseToClient(false, HttpStatusCode.BadRequest, "Could not convert data", null, string.Empty);

                await _unitOfWork.StaffRepositoryInterface.CreateRangeAsync(staffs);

                foreach (var staff in staffs)
                {
                    var schoolUser = _libraryDbContext.SchoolUsers.FirstOrDefault(x => x.UserId == staff.UserId && x.Role.Contains("Staff"));
                    if (schoolUser is null)
                    {
                        await _unitOfWork.StaffRepositoryInterface.DeleteAsync(staff);
                        continue;
                    }

                    var user = _libraryDbContext.Users.FirstOrDefault(x => x.Id == schoolUser.UserId);
                    if (user is null)
                    {
                        await _unitOfWork.StaffRepositoryInterface.DeleteAsync(staff);
                        continue;
                    }

                    string rawPassword = Generators.GenerateRandomPassword();
                    var hashPassword = Generators.CreatePasswordHashWithSalt(rawPassword);
                    user.Password = hashPassword.PasswordHash;
                    user.PasswordSalt = hashPassword.Salt;
                    _libraryDbContext.Users.Update(user);

                    string userDetails = "Kindly find your login details below:\n\n" +
                                        $"Username: {user.Username}.\n" +
                                        $"Password: {rawPassword}.\n\n" +
                                        "You may change your password after logging in.";
                    await _emailSenderService.SendEmailAsync(staff.Email, "Your Login Details", userDetails, staff.FirstName);
                }

                _unitOfWork.Save();
                await _unitOfWork.CommitAsync();

                var result = _apiResponse.ResponseToClient(true, HttpStatusCode.Created, "Successfully added all datas", null, string.Empty);
                return Ok(result);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error adding data", null, ex.Message);
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<APIResponse>> NewStaff([FromBody] StaffDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest("Fields cannot be null");

                if (!ModelState.IsValid)
                    return BadRequest();

                Staff staff = _mapper.Map<Staff>(dto);
                //var stafCapacity = await _unitOfWork.StaffRepositoryInterface.AddUserCapacity(dto);
                //staff.UserCapacities.Add(stafCapacity);
                staff.IsActive = true;
                staff.IsDeleted = false;
                staff.CreatedDate = DateTime.Now;
                staff.ModifiedDate = DateTime.Now;
                staff.CreatedDateOnString = staff.CreatedDate.ToString("dd/MM/yyyy");
                staff.ModifiedDateOnString = staff.ModifiedDate.ToString("dd/MM/yyyy");
                await _unitOfWork.StaffRepositoryInterface.CreateAsync(staff);
                _unitOfWork.Save();
                dto.Id = staff.Id;

                var schoolUser = _libraryDbContext.SchoolUsers.SingleOrDefault(x => x.UserId == staff.UserId && x.Role.Contains("Staff"));
                if (schoolUser is null)
                {
                    await _unitOfWork.StaffRepositoryInterface.DeleteAsync(staff);
                    return _apiResponse.ResponseToClient(false, HttpStatusCode.BadRequest, "Error adding data", null, string.Empty);
                }

                var user = _libraryDbContext.Users.FirstOrDefault(x => x.Id == staff.UserId);
                if (user is null)
                {
                    await _unitOfWork.StaffRepositoryInterface.DeleteAsync(staff);
                    return _apiResponse.ResponseToClient(false, HttpStatusCode.BadRequest, "Not found", null, string.Empty);
                }


                string rawPassword = Generators.GenerateRandomPassword();
                var hashPassword = Generators.CreatePasswordHashWithSalt(rawPassword);

                user.Password = hashPassword.PasswordHash;
                user.PasswordSalt = hashPassword.Salt;
                _libraryDbContext.Users.Update(user);
                await _libraryDbContext.SaveChangesAsync();

                string userDetails = "Kindly find your login details below:\n\n" +
                                    $"Username: {user.Username}.\n" +
                                    $"Password: {rawPassword}.\n\n" +
                                    "You may change your password after logging in.";
                await _emailSenderService.SendEmailAsync(staff.Email, "Your Login Details", userDetails, staff.FirstName);


                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.Created, "Successfully added data", null, string.Empty);
                return CreatedAtAction("GetStaffById", new { Id = dto.Id }, response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error adding data", null, ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<APIResponse>> UpdateStaff([FromBody] StaffDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest("Fields cannot be null");

                var existingResult = await _unitOfWork.StaffRepositoryInterface.GetAsync(dh => dh.Id == dto.Id);
                if (existingResult == null)
                    return NotFound("Staff not found");

                existingResult = _mapper.Map<Staff>(dto);
                existingResult.ModifiedDate = DateTime.Now;
                existingResult.ModifiedDateOnString = existingResult.ModifiedDate.ToString("dd/MM/yyyy");
                await _unitOfWork.StaffRepositoryInterface.UpdateAsync(existingResult);
                _unitOfWork.Save();

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully updated data", null, string.Empty);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error updating data", null, ex.Message);
            }
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult<APIResponse>> DeleteStaff(int Id)
        {
            try
            {
                if (Id == 0)
                    return BadRequest("Id cannot be less than or equal to zero");

                var existingResult = await _unitOfWork.StaffRepositoryInterface.GetAsync(x => x.Id == Id);
                if (existingResult == null)
                    return NotFound("No staff found with the provided Id");

                existingResult.IsDeleted = true;
                await _unitOfWork.StaffRepositoryInterface.DeleteAsync(existingResult);
                _unitOfWork.Save();

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully deleted data", true, string.Empty);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error deleting data", null, ex.Message);
            }

        }
    }
}
