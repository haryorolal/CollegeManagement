using AutoMapper;
using CollegeManagement.Data.Identity;
using CollegeManagement.Data.IRepository;
using CollegeManagement.Data.Model;
using CollegeManagement.Data.Repository;
using CollegeManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CollegeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParentController : ControllerBase
    {
        private readonly ICollegeRepository<Parent> _collegeRepository;
        private readonly IMapper _mapper;
        private APIResponse _apiResponse;
        public ParentController(ICollegeRepository<Parent> collegeRepository, IMapper mapper)
        {
            _collegeRepository = collegeRepository;
            _mapper = mapper;
            _apiResponse = new APIResponse();
        }

        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetAllParents()
        {
            try
            {
                var result = await _collegeRepository.GetAllAsync();
                if (result == null)
                    return NotFound("Empty Request");

                return _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully retrieved data", result, string.Empty);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error retrieving data", null, ex.Message);
            }
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<APIResponse>> GetParentById(int Id)
        {
            try
            {
                if (Id == 0)
                    return BadRequest("Empty Request");

                var result = await _collegeRepository.GetAsync(br => br.Id == Id);
                if (result == null)
                    return NotFound("result could not be found");

                SchoolAdminDTO dto = _mapper.Map<SchoolAdminDTO>(result);
                return _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully retrieved data", dto, string.Empty);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error retrieving data", null, ex.Message);
            }

        }

        [HttpPost("create")]
        public async Task<ActionResult<APIResponse>> NewParent([FromBody] ParentDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest("Empty Request");

                if (!ModelState.IsValid)
                    return BadRequest();

                Parent parent = _mapper.Map<Parent>(dto);
                parent.CreatedDate = DateTime.Now;

                dto.Id = parent.Id;
                var result = await _collegeRepository.CreateAsync(parent);

                APIResponse response = _apiResponse.ResponseToClient(true, HttpStatusCode.Created, "Successfully added data", dto, string.Empty);
                return CreatedAtAction("GetAdminById", new { Id = dto.Id }, response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error adding data", null, ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<APIResponse>> UpdateParent([FromBody] ParentDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest("Empty Request");

                var existingResult = await _collegeRepository.GetAsync(c => c.Id == dto.Id, null, true);
                if (existingResult == null)
                    return NotFound("Data could not be found");

                existingResult = _mapper.Map<Parent>(dto);
                dto.Id = existingResult.Id;
                await _collegeRepository.UpdateAsync(existingResult);

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully updated data", dto, string.Empty);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error updating data", null, ex.Message);
            }
        }


        [HttpDelete("{Id}")]
        public async Task<ActionResult<APIResponse>> DeleteParent(int Id)
        {
            try
            {
                if (Id == 0)
                    return BadRequest("Empty Request");

                var existingResult = await _collegeRepository.GetAsync(d => d.Id == Id);
                if (existingResult == null)
                    return NotFound("data cannot be found");

                await _collegeRepository.DeleteAsync(existingResult);

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
