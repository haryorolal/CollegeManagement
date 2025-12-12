using CollegeManagement.Data.Model;
using CollegeManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using CollegeManagement.Data.IRepository;
using CollegeManagement.Data.Identity;
using Microsoft.AspNetCore.Authorization;

namespace CollegeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class CourseController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly LibraryDbContext _libraryDbContext;
        private readonly IMapper _mapper;
        private readonly APIResponse _apiResponse;
        public CourseController(IUnitOfWork unitOfWork, LibraryDbContext libraryDbContext, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _libraryDbContext = libraryDbContext;
            _mapper = mapper;
            _apiResponse = new APIResponse();
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult<APIResponse>> GetAllCourses()
        {
            try
            {
                var result = await _unitOfWork.CourseRepositoryInterface.GetAllAsync(new List<string> { "CourseLevel", "Department", "Faculty", "School" });
                if (result == null)
                    return NotFound("Looks like there's no data yet");

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully retrieved data", result, string.Empty);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(true, HttpStatusCode.InternalServerError, "Error retrieving data", null, ex.Message);
            }
        }

        [HttpGet("{Id}")]
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<ActionResult<APIResponse>> GetCourseById(int Id)
        {
            try
            {
                if (Id == 0)
                    return BadRequest("Id cannot be less than or equal to zero");

                var result = await _unitOfWork.CourseRepositoryInterface.GetAsync(br => br.Id == Id, new List<string> { "CourseLevel", "Department", "Faculty", "School" });
                if (result == null)
                    return NotFound("No course found with the provided Id");

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully retrieved data", result, string.Empty);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(true, HttpStatusCode.InternalServerError, "Error retrieving data", null, ex.Message);
            }
        }

        [HttpGet("getAllCourseBySchool/{SchoolId}")]
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<ActionResult<APIResponse>> GetCourseBySchoolId(int SchoolId)
        {
            try
            {
                if (SchoolId == 0)
                    return BadRequest("Id cannot be less than or equal to zero");

                 List<Course> result = await _unitOfWork.CourseRepositoryInterface.GetAllFilterAsync(br => br.SchoolId == SchoolId, null, new List<string> { "CourseLevel", "Department", "Faculty", "School" });
                if (result == null)
                    return NotFound("No course found with the provided Id");

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully retrieved data", result, string.Empty);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(true, HttpStatusCode.InternalServerError, "Error retrieving data", null, ex.Message);
            }
        }

        [HttpGet("getAllCourseBySchoolId/facultyId/departmentId/{SchoolId}/{FacultyId}/{DepartmentId}")]
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<ActionResult<APIResponse>> GetCourseByMultipleId(int SchoolId, int FacultyId, int DepartmentId)
        {
            try
            {
                if (SchoolId == 0 && FacultyId == 0 && DepartmentId == 0)
                    return BadRequest("Id cannot be less than or equal to zero");

                List<Course> result = await _unitOfWork.CourseRepositoryInterface.GetAllFilterAsync(br => br.SchoolId == SchoolId && br.FacultyId == FacultyId && br.DepartmentId == DepartmentId);
                if (result == null)
                    return NotFound("No course found with the provided Id");

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully retrieved data", result, string.Empty);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(true, HttpStatusCode.InternalServerError, "Error retrieving data", null, ex.Message);
            }
        }

        [HttpPost("create")]
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<ActionResult<APIResponse>> NewCourse([FromBody] CourseDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest("Fields cannot be null");

                if (!ModelState.IsValid)
                    return BadRequest();

                Course course = _mapper.Map<Course>(dto);
                dto.Id = course.Id;
                await _unitOfWork.CourseRepositoryInterface.CreateAsync(course);

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.Created, "Successfully updated data", true, string.Empty);

                return CreatedAtAction("GetCourseById", new { Id = dto.Id }, response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(true, HttpStatusCode.InternalServerError, "Error adding data", null, ex.Message);
            }
        }

        [HttpPut]
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<ActionResult<APIResponse>> UpdateCourse([FromBody] CourseDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest("Fields cannot be null");
                if (!ModelState.IsValid)
                    return BadRequest();

                var existingResult = await _unitOfWork.CourseRepositoryInterface.GetAsync(c => c.Id == dto.Id, null, true);
                if (existingResult == null)
                    return NotFound("No course found with the provided Id");

                existingResult = _mapper.Map<Course>(dto);
                dto.Id = existingResult.Id;
                await _unitOfWork.CourseRepositoryInterface.UpdateAsync(existingResult);

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully updated data", true, string.Empty);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error updating data", null, ex.Message);
            }
        }

        [HttpDelete("{Id}")]
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<ActionResult<APIResponse>> DeleteCourse(int Id)
        {
            try
            {
                if (Id == 0)
                    return BadRequest("Id cannot be less than or equal to zero");

                var course = await _unitOfWork.CourseRepositoryInterface.GetAsync(x => x.Id == Id);
                if (course == null)
                    return NotFound("No course found with the provided Id");

                var result = await _unitOfWork.CourseRepositoryInterface.UpdateAsync(course);
                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.NoContent, "Successfully deleted data", result, string.Empty);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error deleting data", null, ex.Message);
            }

        }
    }
}
