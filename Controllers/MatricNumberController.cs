using CollegeManagement.Data.Model;
using CollegeManagement.Data;
using CollegeManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using CollegeManagement.Data.IRepository;
using CollegeManagement.Data.Repository;
using Microsoft.AspNetCore.Authorization;

namespace CollegeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class MatricNumberController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly APIResponse _apiResponse;
        private readonly ILogger<MatricNumberController> _logger;
        public MatricNumberController(IUnitOfWork unitOfWork, IMapper mapper, ILogger<MatricNumberController> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _apiResponse = new APIResponse();
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetAllMatricNumber()
        {
            _logger.LogInformation($"Getting all Matriculation Numbers");
            try
            {
                var result = await _unitOfWork.MatricRepositoryInterface.GetAllAsync(new List<string> {"Student"});
                if (result == null)
                    return _apiResponse.ResponseToClient(true, HttpStatusCode.NotFound, "Looks like there's not data yet", result, string.Empty);

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully retrieved data", result, string.Empty);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting all matriculation Numbers");
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error retrieving data", null, ex.Message);
            }
        }

        [HttpGet("school/{schoolId}")]
        public async Task<ActionResult<APIResponse>> GetAllMatricNumberBySchool(int schoolId)
        {
            try
            {
                var result = await _unitOfWork.MatricRepositoryInterface.GetAllFilterAsync(c => c.Student.SchoolId == schoolId, null, new List<string> { "Student" });
                if (result == null)
                    return NotFound("Looks like there's not data yet");

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully retrieved data", result, string.Empty);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error retrieving data", null, ex.Message);
            }
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<APIResponse>> GetMatricNumberById(int Id)
        {
            try
            {
                if (Id == 0)
                    return BadRequest("Id cannot be less than or equal to zero");

                var result = await _unitOfWork.MatricRepositoryInterface.GetAsync(f => f.Id == Id, new List<string> { "Student"}, false);
                if (result == null)
                    return NotFound("Looks like there's no data yet");

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully retrieved data", result, string.Empty);
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error retrieving data", null, ex.Message);
            }
        }

        

        [HttpPost("create")]
        public async Task<ActionResult<APIResponse>> NewMatricNumber([FromBody] MatricNumberDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest("Fields cannot be null");

                if (!ModelState.IsValid)
                    return BadRequest();

                MatricNumber matNum = _mapper.Map<MatricNumber>(dto);
                await _unitOfWork.MatricRepositoryInterface.CreateAsync(matNum);
                _unitOfWork.Save();
                dto.Id = matNum.Id;

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.Created, "Successfully added data", null, string.Empty);

                return CreatedAtAction("GetMatricNumberById", new { Id = dto.Id }, response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error adding data", null, ex.Message);
            }
        }

        
        [HttpGet("generate")]
        public async Task<ActionResult<string>> GenerateMatricNumber()
        {
            var generatedMatricNumber = await _unitOfWork.MatricRepositoryInterface.GenerateMatricNumber();
            return Ok(new { matricNumber = generatedMatricNumber } );
        }

        [HttpPut]
        public async Task<ActionResult<APIResponse>> UpdateMatricNumber([FromBody] MatricNumberDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest("Fields cannot be null");

                var existingResult = await _unitOfWork.MatricRepositoryInterface.GetAsync(dh => dh.Id == dto.Id);
                if (existingResult == null)
                    return NotFound("No matric number found with the provided Id");

                existingResult = _mapper.Map<MatricNumber>(dto);
                await _unitOfWork.MatricRepositoryInterface.UpdateAsync(existingResult);
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
        public async Task<ActionResult<APIResponse>> DeleteMatricNumber(int Id)
        {
            try
            {
                if (Id == 0)
                    return BadRequest("Id cannot be null");

                var existingResult = await _unitOfWork.MatricRepositoryInterface.GetAsync(d => d.Id == Id);
                if (existingResult == null)
                    return BadRequest("No matric number found with the provided Id");

                await _unitOfWork.MatricRepositoryInterface.DeleteAsync(existingResult);
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
