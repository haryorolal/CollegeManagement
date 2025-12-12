using AutoMapper;
using CollegeManagement.Data;
using CollegeManagement.Data.HelperMethod;
using CollegeManagement.Data.Identity;
using CollegeManagement.Data.IRepository;
using CollegeManagement.Data.IServices;
using CollegeManagement.Data.Model;
using CollegeManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CollegeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin, Admin")]
    //[EnableCors(PolicyName = "AllowOnlyLocalhost")]
    public class StudentController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private APIResponse _apiResponse;
        private readonly IUploadExcel<StudentDTO> _uploadExcel;
        private readonly IEmailSenderService _emailSenderService;
        private readonly LibraryDbContext _libraryDbContext;
        public StudentController(IUnitOfWork unitOfWork, IMapper mapper, IUploadExcel<StudentDTO> uploadExcel, IEmailSenderService emailSenderService, LibraryDbContext libraryDbContext)
        {
            _unitOfWork = unitOfWork;
            _uploadExcel = uploadExcel;
            _mapper = mapper;
            _apiResponse = new APIResponse();
            _emailSenderService = emailSenderService;
            _libraryDbContext = libraryDbContext;
        }
               

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetAllStudents()
        {
            try
            {
                var result = await _unitOfWork.StudentRepositoryInterface.GetAllAsync(new List<string> { "User", "Department", "AcademicDuration", "School" });
                if (result == null)
                    return NotFound("No students found");

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully retrieved data", result, string.Empty);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(true, HttpStatusCode.InternalServerError, "Error retrieving data", null, ex.Message);
            }
        }

        [HttpGet("schoolId/{schoolId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetAllStudentsBySchool(int schoolId)
        {
            try
            {
                var result = await _unitOfWork.StudentRepositoryInterface.GetAllFilterAsync(x => x.SchoolId == schoolId && !x.IsDeleted, null, new List<string> { "User", "Department", "AcademicDuration", "School" });
                if (result == null)
                    return NotFound("No students found");

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully retrieved data", result, string.Empty);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(true, HttpStatusCode.InternalServerError, "Error retrieving data", null, ex.Message);
            }
        }

        [HttpGet("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetStudentById(int Id)
        {
            try
            {
                if (Id == 0)
                    return BadRequest("Looks like Id doesn't exist");

                var existingResult = await _unitOfWork.StudentRepositoryInterface.GetAsync(stud => stud.Id == Id, new List<string> { "Books", "Courses" }, false);
                if (existingResult == null)
                    return NotFound($"Student {Id} cannot be found");

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully retrieved data", existingResult, string.Empty);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(true, HttpStatusCode.InternalServerError, "Error retrieving data", null, ex.Message);
            }
        }


        [HttpGet("mat/{MatricId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetStudentByMatricNumber(string MatricId)
        {
            try
            {
                if (MatricId == null)
                    return BadRequest("Looks like Name is not included");

                var existingResult = await _unitOfWork.StudentRepositoryInterface.GetAsync(x => x.MatricNumber.Number == MatricId, new List<string> { "MatricNumber" });
                if (existingResult == null)
                    return NotFound($"Student {MatricId} cannot be found");

                StudentDTO dto = _mapper.Map<StudentDTO>(existingResult);

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully retrieved data", existingResult, string.Empty);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(true, HttpStatusCode.InternalServerError, "Error retrieving data", null, ex.Message);
            }
        }


        [HttpGet("search/{SearchBy}/{SearchText}")]
        public async Task<ActionResult<APIResponse>> SearchDepartment(string SearchBy, string SearchText)
        {
            if (SearchBy == null || SearchText == null)
                return _apiResponse.ResponseToClient(false, HttpStatusCode.BadRequest, "Fields cannot be empty", null, string.Empty);

            List<Student> result = null;
            string[] searchItems = new[] { "LastName", "MatricNumber" };

            // Normalize SearchBy to correct casing then remove the looping
            //SearchBy = searchItems.FirstOrDefault(s => s.Equals(SearchBy, StringComparison.OrdinalIgnoreCase));

            //or 

            // Capitalize first letter to match property names
            SearchBy = char.ToUpper(SearchBy[0]) + SearchBy.Substring(1);

            for (var i = 0; i < searchItems.Length; i++)
            {
                if (SearchBy == searchItems[i])
                    result = await _unitOfWork.StudentRepositoryInterface.GetAllFilterAsync(x => EF.Property<string>(x, SearchBy).ToLower().Contains(SearchText));
            }

            if (result == null)
                return _apiResponse.ResponseToClient(false, HttpStatusCode.NotFound, "Could not find any data related", null, string.Empty);

            var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully retrieved related data", result, string.Empty);
            return Ok(response);
        }

        [HttpPost("upload-excel")]
        public async Task<ActionResult<APIResponse>> UploadStudents(IFormFile file)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var itemDto = await _uploadExcel.ImportExcelAsync(file);
                if (itemDto == null || itemDto.Count == 0)
                    return _apiResponse.ResponseToClient(false, HttpStatusCode.BadRequest, "No data found in the file", null, string.Empty);

                var students = _mapper.Map<List<Student>>(itemDto);
                if (students == null)
                    return _apiResponse.ResponseToClient(false, HttpStatusCode.BadRequest, "Could not convert data", null, string.Empty);

                await _unitOfWork.StudentRepositoryInterface.CreateRangeAsync(students);

                foreach (var student in students)
                {
                    var schoolUser = _libraryDbContext.SchoolUsers.FirstOrDefault(x => x.UserId == student.UserId && x.Role.Contains("Student"));
                    if (schoolUser is null)
                    {
                        await _unitOfWork.StudentRepositoryInterface.DeleteAsync(student);
                        continue;
                    }

                    var user = _libraryDbContext.Users.FirstOrDefault(x => x.Id == schoolUser.UserId);
                    if(user is null)
                    {
                        await _unitOfWork.StudentRepositoryInterface.DeleteAsync(student);
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
                    await _emailSenderService.SendEmailAsync(student.Email, "Your Login Details", userDetails, student.FirstName);

                }

                _unitOfWork.Save();
                await _unitOfWork.CommitAsync();

                var result = _apiResponse.ResponseToClient(true, HttpStatusCode.Created, "Successfully added all datas", null, string.Empty);
                return Ok(result);
            }
            catch(Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error adding data", null, ex.Message);
            }
        }

        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> PostStudent([FromBody] StudentDTO dto)
        {
            try
            {
                if (dto == null && !ModelState.IsValid)
                    return _apiResponse.ResponseToClient(false, HttpStatusCode.BadRequest, "Empty Fields ", null, string.Empty);

                Student student = _mapper.Map<Student>(dto);
                student.CreatedDate = DateTime.Now;
                student.ModifiedDate = DateTime.Now;
                student.CreatedDateOnString = student.CreatedDate.ToString("dd/MM/yyyy");
                student.ModifiedDateOnString = student.ModifiedDate.ToString("dd/MM/yyyy");
                //var userCapacity = await _unitOfWork.StudentRepositoryInterface.AddUserCapacity(dto);
                //student.UserCapacities.Add(userCapacity);

                await _unitOfWork.StudentRepositoryInterface.CreateAsync(student);
                _unitOfWork.Save();
                dto.Id = student.Id;

                var schoolUser = _libraryDbContext.SchoolUsers.SingleOrDefault(x => x.UserId == student.UserId && x.Role.Contains("Student"));
                if (schoolUser is null)
                {
                    await _unitOfWork.StudentRepositoryInterface.DeleteAsync(student);
                    return _apiResponse.ResponseToClient(false, HttpStatusCode.BadRequest, "Error adding data", null, string.Empty);
                }

                var user = _libraryDbContext.Users.SingleOrDefault(x => x.Id == student.UserId);
                if (user is null)
                {
                    await _unitOfWork.StudentRepositoryInterface.DeleteAsync(student);
                    return _apiResponse.ResponseToClient(false, HttpStatusCode.BadRequest, "Not found", null, string.Empty);
                }

                string rawPassword = Generators.GenerateRandomPassword();
                var hashPassword = Generators.CreatePasswordHashWithSalt(rawPassword);

                user.Password = hashPassword.PasswordHash;
                user.PasswordSalt = hashPassword.Salt;
                _libraryDbContext.Users.Update(user);
                await _libraryDbContext.SaveChangesAsync();

                string userDetails ="Kindly find your login details below:\n\n" +
                                    $"Username: {user.Username}.\n" +
                                    $"Password: {rawPassword}.\n\n" +
                                    "You may change your password after logging in.";
                await _emailSenderService.SendEmailAsync(student.Email, "Your Login Details", userDetails, student.FirstName);


                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.Created, "Successfully added data", null, string.Empty);        
                return CreatedAtAction("GetStudentById", new { Id = dto.Id }, response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(true, HttpStatusCode.InternalServerError, "Error adding data", null, ex.Message);
            }
        }


        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> PutStudent([FromBody] StudentDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest("Looks like no student data is not included");

                var existingResult = await _unitOfWork.StudentRepositoryInterface.GetAsync(stud => stud.Id == dto.Id, null, true);
                if (existingResult == null)
                    return NotFound("Sudent record cannot be found");

                existingResult = _mapper.Map<Student>(dto);
                existingResult.ModifiedDate = DateTime.Now;
                existingResult.ModifiedDateOnString = existingResult.ModifiedDate.ToString("dd/MM/yyyy");

                await _unitOfWork.StudentRepositoryInterface.UpdateAsync(existingResult);
                _unitOfWork.Save();

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.NoContent, "Successfully updated data", null, string.Empty);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error updating data", null, ex.Message);
            }
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult<APIResponse>> DeleteStudent(int Id)
        {
            try
            {
                if (Id == 0)
                    return BadRequest("Student Id does not exist");

                var existingResult = await _unitOfWork.StudentRepositoryInterface.GetAsync(stud => stud.Id == Id);
                if (existingResult == null)
                    return NotFound("Student Id cannot be found");

                await _unitOfWork.StudentRepositoryInterface.DeleteAsync(existingResult);
                _unitOfWork.Save();

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.NoContent, "Successfully deleted data", true, string.Empty);
                return Ok(response);

            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error deleting data", null, ex.Message);
            }
        }

    }
}
