using AutoMapper;
using CollegeManagement.Data.IRepository;
using CollegeManagement.Data.Model;
using CollegeManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CollegeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class CourseLevelController : ControllerBase
    {
        private readonly ICollegeRepository<CourseLevel> _courseLevelRepo;
        private readonly IMapper _mapper;
        private APIResponse _apiResponse;

        public CourseLevelController(ICollegeRepository<CourseLevel> collegeRepository, IMapper mapper)
        {
            _courseLevelRepo = collegeRepository;
            _mapper = mapper;
            _apiResponse = new APIResponse();
        }

        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetAllCourseLevel()
        {
            try
            {
                var result = await _courseLevelRepo.GetAllAsync(new List<string> { "Courses"});
                if (result == null)
                    return BadRequest("Oops, no data available yet");

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully retrieved data", result, string.Empty);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error retrieving data", null, ex.Message);
            }
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<APIResponse>> GetCourseLevelById(int Id)
        {
            try
            {
                if (Id == 0)
                    return BadRequest("No field included");

                var existingResult = await _courseLevelRepo.GetAsync(a => a.Id == Id, new List<string> { "Courses" }, false);
                if (existingResult == null)
                    return NotFound("Data not found");

                var respons = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully retrieved data", existingResult, string.Empty);
                return Ok(respons);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error retrieving data", null, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<APIResponse>> AddNewCourseLevel([FromBody] CourseLevelDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest("No field included");

                if (!ModelState.IsValid)
                    return BadRequest("Field is invalid");

                var convertDTO = _mapper.Map<CourseLevel>(dto);
                dto.Id = convertDTO.Id;
                var result = await _courseLevelRepo.CreateAsync(convertDTO);

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.Created, "Successfully added data", dto, string.Empty);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error adding data", null, ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<APIResponse>> UpdateCourseLevel([FromBody] CourseLevelDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest("No field included");

                var existingResult = await _courseLevelRepo.GetAsync(a => a.Id == dto.Id, null, true);
                if (existingResult == null)
                    return null;

                existingResult = _mapper.Map<CourseLevel>(dto);
                dto.Id = existingResult.Id;
                await _courseLevelRepo.UpdateAsync(existingResult);
                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully updated data", null, string.Empty);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error updating data", null, ex.Message);
            }
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult<APIResponse>> DeleteCourseLevel(int Id)
        {
            try
            {
                if (Id == 0)
                    return BadRequest("No field included");

                var existingResult = await _courseLevelRepo.GetAsync(d => d.Id == Id);
                if (existingResult == null)
                    return NotFound("No result found");

                var result = await _courseLevelRepo.DeleteAsync(existingResult);
                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully deleted data", result, string.Empty);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error deleting data", null, ex.Message);
            }
        }


    }
}
