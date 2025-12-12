using AutoMapper;
using CollegeManagement.Data.IRepository;
using CollegeManagement.Data.Models;
using CollegeManagement.Models;
using CollegeManagement.Data.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CollegeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly ICollegeRepository<Author> _authorRepo;
        private readonly LibraryDbContext _libraryDbContext;
        private APIResponse _apiResponse;
        private readonly IMapper _mapper;

        public AuthorController(LibraryDbContext libraryDbContext, IMapper mapper, ICollegeRepository<Author> authoRepo)
        {
            _authorRepo = authoRepo;
            _libraryDbContext = libraryDbContext;
            _mapper = mapper;
            _apiResponse = new APIResponse();
        }

        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetAuthors()
        {
            try
            {
                var existingAuthors = await _authorRepo.GetAllAsync();
                if (existingAuthors == null)
                    return NotFound("No existing Authors yet");

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully retrieved data", existingAuthors, string.Empty);               

                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(true, HttpStatusCode.InternalServerError, "Error retrieving data", null, ex.Message);
            }
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<APIResponse>> GetAuthorById(int Id)
        {
            try
            {
                if (Id == 0)
                    return BadRequest("Id canot be 0");

                Author existingAuthor = await _authorRepo.GetAsync(a => a.Id == Id);
                if (existingAuthor == null)
                    return NotFound("Id cannot be found");

                AuthorDTO dto = _mapper.Map<AuthorDTO>(existingAuthor);
                var response =  _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully retrieved data", dto, string.Empty);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(true, HttpStatusCode.InternalServerError, "Error retrieving data", null, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<APIResponse>> PostAuthor([FromBody] AuthorDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest("Cannot be null");

                Author author = _mapper.Map<Author>(dto);

                dto.Id = author.Id;
                var result = await _authorRepo.CreateAsync(author);

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully added data", dto, string.Empty);


                return CreatedAtAction(nameof(GetAuthorById), new { Id = dto.Id }, response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(true, HttpStatusCode.InternalServerError, "Error adding data", null, ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<APIResponse>> PutAuthor([FromBody] AuthorDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest("Id canot be 0");

                Author existingAuthor = await _authorRepo.GetAsync(x => x.Id == dto.Id);
                if (existingAuthor == null)
                    return NotFound("Author does not exist");

                existingAuthor = _mapper.Map<Author>(dto);
                dto.Id = existingAuthor.Id;

                await _authorRepo.UpdateAsync(existingAuthor);

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully updated data", dto, string.Empty);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error updating data", null, ex.Message);
            }
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult<APIResponse>> DeleteAuthor(int Id)
        {
            try
            {
                if (Id == 0)
                    return BadRequest("Id canot be 0");

                Author existingAuthor = await _authorRepo.GetAsync(x => x.Id == Id);
                if (existingAuthor == null)
                    return NotFound("Id cannot be found");

                await _authorRepo.DeleteAsync(existingAuthor);

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
