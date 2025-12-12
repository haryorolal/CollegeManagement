using CollegeManagement.Data.Model;
using CollegeManagement.Data;
using CollegeManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using CollegeManagement.Data.IRepository;

namespace CollegeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibraryCardController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly APIResponse _apiResponse;
        public LibraryCardController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _apiResponse = new APIResponse();
        }

        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetAllLibraryCard()
        {
            try
            {
                var result = await _unitOfWork.LibraryCardRepositoryInterface.GetAllAsync();
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
        public async Task<ActionResult<APIResponse>> GetLibraryCardById(int Id)
        {
            try
            {
                if (Id == 0)
                    return BadRequest("Id cannot be less than or equal to zero");

                var result = await _unitOfWork.LibraryCardRepositoryInterface.GetAsync(f => f.Id == Id);
                if (result == null)
                    return NotFound("Looks like there's no data yet");

                LibraryCardDTO dto = _mapper.Map<LibraryCardDTO>(result);
                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully retrieved data", dto, string.Empty);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error retrieving data", null, ex.Message);
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<APIResponse>> NewLibraryCard([FromBody] LibraryCardDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest("Fields cannot be null");

                if (!ModelState.IsValid)
                    return BadRequest();

                LibraryCard libCard = _mapper.Map<LibraryCard>(dto);
                libCard.CardNumber = await _unitOfWork.LibraryCardRepositoryInterface.GenerateLibraryCard("LIB");

                dto.Id = libCard.Id;
                await _unitOfWork.LibraryCardRepositoryInterface.CreateAsync(libCard);
                _unitOfWork.Save();

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.Created, "Successfully added data", null, string.Empty);


                return CreatedAtAction("GetLibraryCardById", new { Id = dto.Id }, response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(true, HttpStatusCode.InternalServerError, "Error adding data", null, ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<APIResponse>> UpdateLibraryCard([FromBody] LibraryCardDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest("Fields cannot be null");

                var existingResult = await _unitOfWork.LibraryCardRepositoryInterface.GetAsync(x => x.Id == dto.Id, null, true);
                existingResult = _mapper.Map<LibraryCard>(dto);
                dto.Id = existingResult.Id;
                await _unitOfWork.LibraryCardRepositoryInterface.UpdateAsync(existingResult);
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
        public async Task<ActionResult<APIResponse>> DeleteLibraryCard(int Id)
        {
            try
            {
                if (Id == 0)
                    return BadRequest("Id cannot be less than or equal to zero");

                var existingResult = await _unitOfWork.LibraryCardRepositoryInterface.GetAsync(x => x.Id == Id);
                if (existingResult == null)
                    return NotFound("No Department found with the provided Id");

                await _unitOfWork.LibraryCardRepositoryInterface.DeleteAsync(existingResult);
                _unitOfWork.Save();
                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully deleted data", true, string.Empty);

                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error deleting data", null, ex.Message);
            }

        }
    }
}
