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
    public class ExamController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private APIResponse _apiResponse;

        public ExamController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _apiResponse = new APIResponse();
        }

        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetExams()
        {
            try
            {
                var result = await _unitOfWork.ExamRepositoryInterface.GetAllAsync(new List<string> { "Departments"});
                if (result == null)
                    return NotFound("No exams found");

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully retrieved data", result, string.Empty);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error retrieving data", null, ex.Message);
            }
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<APIResponse>> GetExamById(int Id)
        {
            try
            {
                if (Id == 0)
                    return BadRequest("Id cannot be zero");

                var existingResult = await _unitOfWork.ExamRepositoryInterface.GetAsync(x => x.Id == Id, new List<string> { "Departments" }, false);
                if (existingResult == null)
                    return NotFound("Exam not found");

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully retrieved data", existingResult, string.Empty);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error retrieving data", null, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<APIResponse>> NewExam([FromBody] ExamDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest("Data cannot be empty");

                var exam = _mapper.Map<Exam>(dto);
                exam.CreatedDate = DateTime.Now;
                exam.CreatedDateToString = exam.CreatedDate.ToString("yy/MM/yyyy");

                dto.Id = exam.Id;
                var result = await _unitOfWork.ExamRepositoryInterface.CreateAsync(exam);
                _unitOfWork.Save();

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.Created, "Successfully created", result, string.Empty);
                return CreatedAtAction("GetExamById", new { Id = exam.Id }, response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error creating data", null, ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<APIResponse>> UpdateExam([FromBody] ExamDTO examDTO)
        {
            try
            {
                if (examDTO == null)
                    return BadRequest("Invalid data");

                var existingExam = await _unitOfWork.ExamRepositoryInterface.GetAsync(x => x.Id == examDTO.Id, null, true);
                if (existingExam == null)
                    return NotFound("Exam not found");

                var updatedExam = _mapper.Map<Exam>(examDTO);
                examDTO.Id = updatedExam.Id;
                await _unitOfWork.ExamRepositoryInterface.UpdateAsync(updatedExam);
                _unitOfWork.Save();

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully updated", null, string.Empty);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error updating data", null, ex.Message);
            }
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult<APIResponse>> DeleteExam(int Id)
        {
            try
            {
                if (Id <= 0)
                    return BadRequest("Id cannot be zero");

                var existingExam = await _unitOfWork.ExamRepositoryInterface.GetAsync(x => x.Id == Id);
                if (existingExam == null)
                    return NotFound("Exam not found");

                await _unitOfWork.ExamRepositoryInterface.DeleteAsync(existingExam);
                _unitOfWork.Save();
                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully deleted", true, string.Empty);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error deleting data", null, ex.Message);
            }
        }


    }
}
