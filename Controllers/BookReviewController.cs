using AutoMapper;
using CollegeManagement.Data;
using CollegeManagement.Data.IRepository;
using CollegeManagement.Data.Model;
using CollegeManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CollegeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookReviewController : ControllerBase
    {
        private readonly ICollegeRepository<BookReview> _bookReviewDbContext;
        private readonly IMapper _mapper;
        private readonly APIResponse _apiResponse;
        public BookReviewController(ICollegeRepository<BookReview> bookReviewRepo, IMapper mapper)
        {
            _bookReviewDbContext = bookReviewRepo;
            _mapper = mapper;
            _apiResponse = new APIResponse();
        }

        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetBookReview()
        {
           try
            {
                var result = await _bookReviewDbContext.GetAllAsync();

                if (result == null)
                    return NotFound("Looks like there's no book reviews yet.");

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully retrieved data", result, string.Empty);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error retrieving data", null, ex.Message);
            }
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<APIResponse>> GetBookReviewById(int Id)
        {
            try
            {
                if (Id == 0)
                    return BadRequest("Id cannot be less than or equal to zero");

                var result = await _bookReviewDbContext.GetAsync(br => br.Id == Id);
                if (result == null)
                    return NotFound("Could not be found");

                //BookReviewDTO dto = _mapper.Map<BookReviewDTO>(result);
                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully retrieved data", result, string.Empty);

                return Ok(response);

            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error retrieving data", null, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<APIResponse>> AddNewBookReview([FromBody] BookReviewDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest("Book review data is null.");

                BookReview bookReview = _mapper.Map<BookReview>(dto);
                bookReview.CreatedDate = DateTime.Now;
                bookReview.CreatedDateTimeOnString = bookReview.CreatedDate.ToString("dd/MM/yyyy");

                dto.Id = bookReview.Id;
                await _bookReviewDbContext.CreateAsync(bookReview);

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.Created, "Successfully added data", dto, string.Empty);

                return CreatedAtAction("GetBookReviewById", new { Id = dto.Id }, response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(true, HttpStatusCode.InternalServerError, "Error adding data", null, ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<APIResponse>> UpdateBookReview([FromBody] BookReviewDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest("Fields cannot be null");

                var existingResult = await _bookReviewDbContext.GetAsync(ac => ac.Id == dto.Id, null, true);
                if (existingResult == null)
                    return NotFound("Could not be found");

                existingResult = _mapper.Map<BookReview>(dto);
                dto.Id = existingResult.Id;
                var result = await _bookReviewDbContext.UpdateAsync(existingResult);

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully updated data", result, string.Empty);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error updating data", null, ex.Message);
            }
        }


        [HttpDelete("{Id}")]
        public async Task<ActionResult<APIResponse>> DeleteBookReview(int Id)
        {
            try
            {
                if (Id == 0)
                    return BadRequest("Id canot be 0");

                BookReview existingResult = await _bookReviewDbContext.GetAsync(x => x.Id == Id);
                if (existingResult == null)
                    return NotFound(" cannot be found");

                var result = await _bookReviewDbContext.DeleteAsync(existingResult);
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
