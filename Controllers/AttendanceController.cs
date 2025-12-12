using AutoMapper;
using CollegeManagement.Data.IRepository;
using CollegeManagement.Data.Model;
using CollegeManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CollegeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly ICollegeRepository<Attendance> _attendanceRepository;
        private readonly IMapper _mapper;
        private APIResponse _apiResponse;

        public AttendanceController(ICollegeRepository<Attendance> collegeRepository, IMapper mapper)
        {
            _attendanceRepository = collegeRepository;
            _mapper = mapper;
            _apiResponse = new APIResponse();
        }

        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetAttendance()
        {
            try
            {
                var result = await _attendanceRepository.GetAllAsync(new List<string> { "Courses", "Students"});
                if (result == null)
                    return NotFound("Oops, no attendance recorded yet");

                return _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully retrieved data", result, string.Empty);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(true, HttpStatusCode.InternalServerError, "Error retrieving data", null, ex.Message);
            }
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<APIResponse>> GetAttendanceById(int Id)
        {
            try
            {
                if (Id == 0)
                    return BadRequest("Id cannot be empty");

                var existingResult = await _attendanceRepository.GetAsync(x => x.Id == Id, new List<string> { "Courses", "Students" }, false);
                if (existingResult == null)
                    return NotFound("Oops, attendance can't be found");

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully retrieved data", existingResult, string.Empty);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(true, HttpStatusCode.InternalServerError, "Error retrieving data", null, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<APIResponse>> AddNewAttendance([FromBody] AttendanceDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest("Empty Fields");

                if (!ModelState.IsValid)
                    return BadRequest("Field is invalid");

                var convertedResult = _mapper.Map<Attendance>(dto);
                convertedResult.CreatedDate = DateTime.Now;
                convertedResult.AttendanceDateCreated = convertedResult.CreatedDate.ToString("dd/MM/yyyy");

                dto.Id = convertedResult.Id;
                var result = await _attendanceRepository.CreateAsync(convertedResult);
                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.Created, "Successfully added data", dto, string.Empty);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(true, HttpStatusCode.InternalServerError, "Error adding data", null, ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<APIResponse>> UpdateAttendance([FromBody] AttendanceDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest("Empty Fields");

                var existingResult = await _attendanceRepository.GetAsync(a => a.Id == dto.Id, null, true);
                if (existingResult == null)
                    return NotFound("Attendance can't be found");

                dto.Id = existingResult.Id;
                var result = await _attendanceRepository.UpdateAsync(existingResult);
                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully updated data", result, string.Empty);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error updating data", null, ex.Message);
            }
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult<APIResponse>> DeleteAttendance(int Id)
        {
            try
            {
                if (Id == 0)
                    return BadRequest("Empty Fields");

                var existingResult = await _attendanceRepository.GetAsync(d => d.Id == Id);
                if (existingResult == null)
                    return NotFound("Attendance cannot be found");

                var result = _attendanceRepository.DeleteAsync(existingResult);
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
