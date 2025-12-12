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
    public class QuestionController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private APIResponse _apiResponse;

        public QuestionController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _apiResponse = new APIResponse();
        }

        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetAllQuestions()
        {
            try
            {
                var result = await _unitOfWork.QuestionRespositoryInterface.GetAllAsync(new List<string> {"Staff", "Course"});
                if (result == null || !result.Any())
                    return NotFound("No questions found");

               var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully retrieved data", result, string.Empty);
               return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error retrieving data", null, ex.Message);
            }
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<APIResponse>> GetQuestionById(int Id)
        {
            try
            {
                if (Id <= 0) 
                    return BadRequest("Id cannot be less than or equal to zero");

                var result = await _unitOfWork.QuestionRespositoryInterface.GetAsync(a => a.Id == Id, new List<string> { "Staff", "Course" }, false);
                if (result == null)
                    return NotFound("Data not found");

                var dto = _mapper.Map<QuestionDTO>(result);
                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully retrieved data", dto, string.Empty);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error retrieving data", null, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<APIResponse>> AddQuestion([FromBody] QuestionDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest("Data cannot be null");

                var result = _mapper.Map<Question>(dto);
                dto.Id = result.Id;

                await _unitOfWork.QuestionRespositoryInterface.CreateAsync(result);
                _unitOfWork.Save();

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.Created, "Successfully added data", null, string.Empty);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error adding data", null, ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<APIResponse>> UpdateQuestion(QuestionDTO dto)
        {
            try
            {
                var existingResult = await _unitOfWork.QuestionRespositoryInterface.GetAsync(a => a.Id == dto.Id, null, true);
                if (existingResult == null)
                    return NotFound("Data not found");

                existingResult = _mapper.Map<Question>(dto);
                dto.Id = existingResult.Id;

                await _unitOfWork.QuestionRespositoryInterface.UpdateAsync(existingResult);
                _unitOfWork.Save();

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully updated data", null, string.Empty);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error updating data", null, ex.Message);
            }
        }

        [HttpDelete]
        public async Task<ActionResult<APIResponse>> DeleteQuestion(int Id)
        {
            try
            {
                if (Id <= 0)
                    return BadRequest("Id cannot be less than or equal to zero");

                var result = await _unitOfWork.QuestionRespositoryInterface.GetAsync(d => d.Id == Id);
                if (result == null)
                    return NotFound("Not found");

                await _unitOfWork.QuestionRespositoryInterface.DeleteAsync(result);
                _unitOfWork.Save();
                return _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully deleted data", true, string.Empty);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error deleting data", null, ex.Message);
            }
        }







    }
}
