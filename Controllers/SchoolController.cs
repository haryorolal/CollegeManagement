using AutoMapper;
using CollegeManagement.Data;
using CollegeManagement.Data.IRepository;
using CollegeManagement.Data.IServices;
using CollegeManagement.Data.Model;
using CollegeManagement.Data.Services;
using CollegeManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CollegeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SchoolController : ControllerBase
    {
        private readonly ICollegeRepository<School> _schoolRepo;
        private readonly IMapper _mapper;
        private readonly IUploadExcel<SchoolDTO> _uploadExcel;
        private readonly APIResponse _apiResponse;
        public SchoolController(ICollegeRepository<School> schoolRepo, IMapper mapper, IUploadExcel<SchoolDTO> uploadExcel)
        {
            _schoolRepo = schoolRepo;
            _mapper = mapper;
            _uploadExcel = uploadExcel;
            _apiResponse = new APIResponse();
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<ActionResult<APIResponse>> GetAllSchool()
        {
            try
            {
                var result = await _schoolRepo.GetAllAsync(new List<string>{"Faculties", "Departments", "Libraries", "SchoolAdmins", "Staffs", "Students", "Courses", "Exams" } );
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
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<ActionResult<APIResponse>> GetSchoolById(int Id)
        {
            try
            {
                if (Id == 0)
                    return _apiResponse.ResponseToClient(false, HttpStatusCode.BadRequest, "Id cannot be less than or equal to zero", null, string.Empty);

                var result = await _schoolRepo.GetAsync(f => f.Id == Id, new List<string> { "Faculties", "Departments", "Libraries","SchoolAdmins", "Staffs", "Students", "Courses", "Exams" }, false);
                if (result == null)
                    return _apiResponse.ResponseToClient(false, HttpStatusCode.BadRequest, "Looks like there's no data yet", null, string.Empty);

                School dto = _mapper.Map<School>(result);

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully retrieved data", dto, string.Empty);
                return Ok(response);

            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error retrieving data", null, ex.Message);
            }
        }

        [HttpGet("search/{SearchBy}/{SearchText}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult<APIResponse>> SearchStudent(string SearchBy, string SearchText)
        {
            if (SearchBy == null || SearchText == null)
                return _apiResponse.ResponseToClient(false, HttpStatusCode.BadRequest, "Fields cannot be empty", null, string.Empty);

            List<School> result = null;
            string[] searchItems = new[] {"Name", "Address", "Founder", "SchoolCode" };

            // Normalize SearchBy to correct casing then remove the looping
            //SearchBy = searchItems.FirstOrDefault(s => s.Equals(SearchBy, StringComparison.OrdinalIgnoreCase));

            //or 

            // Capitalize first letter to match property names
            SearchBy = char.ToUpper(SearchBy[0]) + SearchBy.Substring(1);

            for (var i = 0; i < searchItems.Length; i++)
            {
                if (SearchBy == searchItems[i])
                    result = await _schoolRepo.GetAllFilterAsync( x => EF.Property<string>(x, SearchBy).ToLower().Contains(SearchText), null, new List<string> { "Faculties", "Departments", "Libraries", "SchoolAdmins", "Staffs", "Students", "Courses", "Exams" }, false);
            }

            if (result == null)
                return _apiResponse.ResponseToClient(false, HttpStatusCode.NotFound, "Could not find any data related", null, string.Empty);

            var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully retrieved related data", result, string.Empty);
            return Ok(response);
        }

        [HttpPost("create")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult<APIResponse>> NewSchool([FromBody] SchoolDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest("Fields cannot be null");

                if (!ModelState.IsValid)
                    return BadRequest();

                School sch = _mapper.Map<School>(dto);

                dto.Id = sch.Id;
                await _schoolRepo.CreateAsync(sch);

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.Created, "Successfully added data", null, string.Empty);
                return CreatedAtAction("GetSchoolById", new { Id = dto.Id }, response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error adding data", null, ex.Message);
            }
        }

        [HttpPost("upload-excel")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult<APIResponse>> UploadSchool(IFormFile file)
        {
            var sschoolDto = await _uploadExcel.ImportExcelAsync(file);
            if (sschoolDto == null || sschoolDto.Count == 0)
                return _apiResponse.ResponseToClient(false, HttpStatusCode.BadRequest, "No data found in the file", null, string.Empty);

            var schools = _mapper.Map<List<School>>(sschoolDto);
            await _schoolRepo.CreateRangeAsync(schools);

            var result = _apiResponse.ResponseToClient(true, HttpStatusCode.Created, "Successfully added all datas", null, string.Empty);
            return Ok(result);
        }

        [HttpPut]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult<APIResponse>> UpdateSchool([FromBody] SchoolDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest("Fields cannot be null");

                var existingResult = await _schoolRepo.GetAsync(dh => dh.Id == dto.Id, null, true);
                if (existingResult == null)
                    return NotFound("School record cannot be found");

                existingResult = _mapper.Map<School>(dto);
                dto.Id = existingResult.Id;

                await _schoolRepo.UpdateAsync(existingResult);
                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully updated data", null, string.Empty);


                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error updating data", null, ex.Message);
            }
        }

        [HttpDelete("{Id}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult<APIResponse>> DeleteSchool(int Id)
        {
            try
            {
                if (Id == 0)
                    return BadRequest("Id cannot be less than or equal to zero");

                var existingResult = await _schoolRepo.GetAsync(x => x.Id == Id);
                if (existingResult == null)
                    return NotFound("No school found with the provided Id");

                await _schoolRepo.DeleteAsync(existingResult);

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
