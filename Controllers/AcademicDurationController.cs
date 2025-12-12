using CollegeManagement.Data.Model;
using CollegeManagement.Data;
using CollegeManagement.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using CollegeManagement.Data.IRepository;
using Microsoft.AspNetCore.Authorization;
using CollegeManagement.Data.Repository;
using CollegeManagement.Data.IServices;

namespace CollegeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin, SuperAdmin")]
    public class AcademicDurationController : ControllerBase
    {
        private readonly ICollegeRepository<AcademicDuration> _academicDurationRepo;
        private readonly IMapper _mapper;
        private APIResponse _apiResponse;
        private readonly IUploadExcel<AcademicDurationDTO> _uploadExcel;
        public AcademicDurationController(ICollegeRepository<AcademicDuration> academicDurationRepo, IMapper mapper, IUploadExcel<AcademicDurationDTO> uploadExcel)
        {
            _apiResponse = new APIResponse();
            _academicDurationRepo = academicDurationRepo;
            _mapper = mapper;
            _uploadExcel = uploadExcel;
        }

        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetAllAcademicDuration()
        {
            try
            {
                var result = await _academicDurationRepo.GetAllAsync(new List<string> { "Department", "Students" });
                if (result == null)
                    return NotFound("Empty Request");

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully retrieved data", result, string.Empty);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error retrieving data", null, ex.Message);
            }
        }

        [HttpGet("schoolId/{schoolId}")]
        public async Task<ActionResult<APIResponse>> GetAcademicDurationBySchool(int schoolId)
        {
            try
            {
                if (schoolId == 0)
                    return BadRequest("Empty Request");

                List<AcademicDuration> result = await _academicDurationRepo.GetAllFilterAsync(br => br.Department.SchoolId == schoolId, null, new List<string> { "Department", "Students" });
                if (result == null)
                    return NotFound("result could not be found");

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully retrieved data", result, string.Empty);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error retrieving data", null, ex.Message);
            }
        }

        [HttpGet("department/{departmentId}")]
        public async Task<ActionResult<APIResponse>> GetAcademicDurationByDepartment(int departmentId)
        {
            try
            {
                if (departmentId == 0)
                    return BadRequest("Empty Request");

                List<AcademicDuration> result = await _academicDurationRepo.GetAllFilterAsync(br => br.Department.Id == departmentId);
                if (result == null)
                    return NotFound("result could not be found");

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully retrieved data", result, string.Empty);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error retrieving data", null, ex.Message);
            }
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<APIResponse>> GetAcademicDurationById(int Id)
        {
            try
            {
                if (Id == 0)
                    return BadRequest("Empty Request");

                var result = await _academicDurationRepo.GetAsync(br => br.Id == Id);
                if (result == null)
                    return NotFound("result could not be found");

                AcademicDurationDTO dto = _mapper.Map<AcademicDurationDTO>(result);
                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully retrieved data", dto, string.Empty);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error retrieving data", null, ex.Message);
            }
            
        }

        [HttpGet("search/{SearchBy}/{SearchText}")]
        public async Task<ActionResult<APIResponse>> SearchDepartment(string SearchBy, string SearchText)
        {
            if (SearchBy == null || SearchText == null)
                return _apiResponse.ResponseToClient(false, HttpStatusCode.BadRequest, "Fields cannot be empty", null, string.Empty);

            List<AcademicDuration> result = null;
            string[] searchItems = new[] { "TotalYears", "Name" };

            // Normalize SearchBy to correct casing then remove the looping
            //SearchBy = searchItems.FirstOrDefault(s => s.Equals(SearchBy, StringComparison.OrdinalIgnoreCase));

            //or 

            // Capitalize first letter to match property names
            SearchBy = char.ToUpper(SearchBy[0]) + SearchBy.Substring(1);

            for (var i = 0; i < searchItems.Length; i++)
            {
                if (SearchBy == searchItems[i])
                    result = await _academicDurationRepo.GetAllFilterAsync(x => EF.Property<string>(x, SearchBy).ToLower().Contains(SearchText));
            }

            if (result == null)
                return _apiResponse.ResponseToClient(false, HttpStatusCode.NotFound, "Could not find any data related", null, string.Empty);

            var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully retrieved related data", result, string.Empty);
            return Ok(response);
        }

        [HttpPost("upload-excel")]
        public async Task<ActionResult<APIResponse>> UploadDepartment(IFormFile file)
        {
            var dto = await _uploadExcel.ImportExcelAsync(file);
            if (dto == null || dto.Count == 0)
                return _apiResponse.ResponseToClient(false, HttpStatusCode.BadRequest, "No data found in the file", null, string.Empty);

            var duration = _mapper.Map<List<AcademicDuration>>(dto);
            if (duration == null)
                return _apiResponse.ResponseToClient(false, HttpStatusCode.BadRequest, "Could not convert data", null, string.Empty);

            await _academicDurationRepo.CreateRangeAsync(duration);

            var result = _apiResponse.ResponseToClient(true, HttpStatusCode.Created, "Successfully added all datas", null, string.Empty);
            return Ok(result);
        }



        [HttpPost("create")]
        public async Task<ActionResult<APIResponse>> NewAcademicDuration([FromBody] AcademicDurationDTO dto)
        {           
            try
            {
                if (dto == null)
                    return BadRequest("Empty Request");

                if (!ModelState.IsValid)
                    return BadRequest();

                AcademicDuration AcDuration = _mapper.Map<AcademicDuration>(dto);
                var result = await _academicDurationRepo.CreateAsync(AcDuration);
                dto.Id = AcDuration.Id;

                APIResponse response = _apiResponse.ResponseToClient(true, HttpStatusCode.Created, "Successfully added data", dto, string.Empty);
                return CreatedAtAction("GetAcademicDurationById", new { Id = dto.Id }, response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error adding data", null, ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<APIResponse>> UpdateAcademicDuration([FromBody] AcademicDurationDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest("Empty Request");

                var existingResult = await _academicDurationRepo.GetAsync(c => c.Id == dto.Id, null, true);
                if (existingResult == null)
                    return NotFound("Data could not be found");

                existingResult = _mapper.Map<AcademicDuration>(dto);
                await _academicDurationRepo.UpdateAsync(existingResult);
                dto.Id = existingResult.Id;

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully updated data", dto, string.Empty);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error updating data", null, ex.Message);
            }
        }


        [HttpDelete("{Id}")]
        public async Task<ActionResult<APIResponse>> DeleteAcademicDuration(int Id)
        {
            try
            {
                if (Id == 0)
                    return BadRequest("Empty Request");

                var existingAcademicDuration = await _academicDurationRepo.GetAsync(d => d.Id == Id);
                if (existingAcademicDuration == null)
                    return NotFound("data cannot be found");

                await _academicDurationRepo.DeleteAsync(existingAcademicDuration);

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
