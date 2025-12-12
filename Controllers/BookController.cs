using AutoMapper;
using CollegeManagement.Data;
using CollegeManagement.Data.IRepository;
using CollegeManagement.Data.Models;
using CollegeManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CollegeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private APIResponse _apiResponse;
        public BookController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _apiResponse = new APIResponse();
        }

        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetBooks()
        {
            try
            {
                var result = await _unitOfWork.BookRepositoryInterface.GetAllAsync(new List<string> {"Author", "BookReviews" });                
                if (result == null)
                    return NotFound("No books found yet");

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully retrieved data", result, string.Empty);
                return Ok(response);

            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(true, HttpStatusCode.InternalServerError, "Error retrieving data", null, ex.Message);
            }
        }

        [HttpGet("getbooksbyreviews/popular")]
        public async Task<ActionResult<APIResponse>> GetBooksByReviews()
        {
            try
            {
                var result = await _unitOfWork.BookRepositoryInterface.GetBookByReviews();
                if (result == null)
                    return NotFound("No books found yet");

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully retrieved data", result, string.Empty);
                return Ok(response);

            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(true, HttpStatusCode.InternalServerError, "Error retrieving data", null, ex.Message);
            }
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<APIResponse>> GetBookById(int Id)
        {
            try
            {
                if (Id == 0)
                    return BadRequest("Id canot be 0");

                Book existingBook = await _unitOfWork.BookRepositoryInterface.GetAsync(a => a.Id == Id, new List<string> { "Author", "BookReviews", "BookReviews.Student"});
                if (existingBook == null)
                    return NotFound("Id cannot be found");

                //BookDTO dto = _mapper.Map<BookDTO>(existingBook);
                
                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully retrieved data", existingBook, string.Empty);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error retrieving data", null, ex.Message);
            }
        }

        [HttpGet("author/{AuthorId}")]
        public async Task<ActionResult<APIResponse>> GetBookByAuthorId(int AuthorId)
        {
            try
            {
                if (AuthorId == 0)
                    return BadRequest("Id canot be 0");

                Book existingBook = await _unitOfWork.BookRepositoryInterface.GetAsync(a => a.AuthorId == AuthorId, new List<string> { "Author", "BookReviews", "BookReviews.Student" });
                if (existingBook == null)
                    return NotFound("Id cannot be found");

                //BookDTO dto = _mapper.Map<BookDTO>(existingBook);
                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully retrieved data", existingBook, string.Empty);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error retrieving data", null, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<APIResponse>> PostBook([FromBody] BookDTO bookDTO)
        {
            try
            {
                if (bookDTO == null)
                    return BadRequest("Cannot be null");

                Book books = _mapper.Map<Book>(bookDTO);

                bookDTO.Id = books.Id;                
                var result = await _unitOfWork.BookRepositoryInterface.CreateAsync(books);
                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.Created, "Successfully added data", result, string.Empty);

                return CreatedAtAction(nameof(GetBookById), new { Id = bookDTO.Id }, response);

            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error adding data", null, ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<APIResponse>> PutBook([FromBody] BookDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest("book data cannot be empty");

                Book existingResult = await _unitOfWork.BookRepositoryInterface.GetAsync(x => x.Id == dto.Id, null, true);
                if (existingResult == null)
                    return NotFound("Author does not exist");

                existingResult = _mapper.Map<Book>(dto);
                dto.Id = existingResult.Id;
                var result = await _unitOfWork.BookRepositoryInterface.UpdateAsync(existingResult);
                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully updated data", result, string.Empty);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error updating data", null, ex.Message);
            }
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult<APIResponse>> DeleteBook(int Id)
        {
            try
            {
                if (Id == 0)
                    return BadRequest("Id canot be 0");

                Book existingBook = await _unitOfWork.BookRepositoryInterface.GetAsync(x => x.Id == Id);
                if (existingBook == null)
                    return NotFound("Id cannot be found");

                var result = await _unitOfWork.BookRepositoryInterface.DeleteAsync(existingBook);
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
