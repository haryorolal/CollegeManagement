using AutoMapper;
using CollegeManagement.Data;
using CollegeManagement.Data.IRepository;
using CollegeManagement.Data.Model;
using CollegeManagement.Models;
using Microsoft.AspNetCore.Authorization;
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
    public class LibraryController : ControllerBase
    {
        private readonly ICollegeRepository<Library> _libraryDbContext;
        private readonly IMapper _mapper;
        private APIResponse _apiResponse;
        public LibraryController(ICollegeRepository<Library> libraryDbContext, IMapper mapper)
        {
            _libraryDbContext = libraryDbContext;
            _mapper = mapper;
            _apiResponse = new APIResponse();
        }

        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetAllStudents()
        {
            try
            {
                var libraries = await _libraryDbContext.GetAllAsync(new List<string> { "Books", "Students", "LibraryCards"});
                if (libraries == null)
                    return NotFound("Looks like there's no data yet");

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully retrieved data", libraries, string.Empty);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error retrieving data", null, ex.Message);
            }
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<APIResponse>> GetLibraryById(int Id)
        {
            try
            {
                if (Id == 0)
                    return BadRequest("Id cannot be null");

                var existingResult = await _libraryDbContext.GetAsync(f => f.Id == Id, new List<string> { "Books", "Students", "LibraryCards" }, false);
                if (existingResult == null)
                    return NotFound("Looks like there's no data yet");

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully retrieved data", existingResult, string.Empty);
                return Ok(response);

            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error retrieving data", null, ex.Message);
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<APIResponse>> CreateLibraryAsync([FromBody] LibraryDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest("library data is null");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                Library library = _mapper.Map<Library>(dto);
                dto.Id = library.Id;
                var result = await _libraryDbContext.CreateAsync(library);

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.Created, "Successfully added data", dto, string.Empty);

                return CreatedAtAction("GetLibraryById", new { Id = dto.Id }, response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error adding data", null, ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<APIResponse>> UpdateLibrary([FromBody] LibraryDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest("Fields cannot be null");

                var existingResult = await _libraryDbContext.GetAsync(dh => dh.Id == dto.Id);
                if (existingResult == null)
                    return NotFound("library does not exist");

                existingResult = _mapper.Map<Library>(dto);
                dto.Id = existingResult.Id;
                await _libraryDbContext.UpdateAsync(existingResult);

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.NoContent, "Successfully updated data", null, string.Empty);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error updating data", null, ex.Message);
            }
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult<APIResponse>> DeleteLibrary(int Id)
        {
            try
            {
                if (Id == 0)
                    return BadRequest("Id cannot be less than or equal to zero");

                var existingResult = await _libraryDbContext.GetAsync(x => x.Id == Id);
                if (existingResult == null)
                    return NotFound("No Library found with the provided Id");

                await _libraryDbContext.DeleteAsync(existingResult);
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
