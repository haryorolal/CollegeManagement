using AutoMapper;
using CollegeManagement.Data.Identity;
using CollegeManagement.Data.IRepository;
using CollegeManagement.Data.IServices;
using CollegeManagement.Data.Model;
using CollegeManagement.Data.Repository;
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
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class AdminController : ControllerBase
    {
        private readonly ICollegeRepository<SchoolAdmin> _collegeRepository;
        private readonly IMapper _mapper;
        private APIResponse _apiResponse;
        private readonly IUploadExcel<SchoolAdminDTO> _uploadExcel;
        public AdminController(ICollegeRepository<SchoolAdmin> collegeRepository, IMapper mapper, IUploadExcel<SchoolAdminDTO> uploadExcel)
        {
            _collegeRepository = collegeRepository;
            _mapper = mapper;
            _apiResponse = new APIResponse();
            _uploadExcel = uploadExcel;
        }

        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetAllSchoolAdmins()
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

        

        [HttpGet("adminby/{schoolId}")]
        public async Task<ActionResult<APIResponse>> GetSchoolAdminBySchoolId(int schoolId)
        {
            try
            {
                if (schoolId == 0)
                    return BadRequest("Empty Request");

                var result = await _collegeRepository.GetAsync(br => br.Id == schoolId, new List<string> { });
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

        [HttpGet("{Id}")]
        public async Task<ActionResult<APIResponse>> GetSchoolAdminById(int Id)
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


        [HttpGet("search/{SearchBy}/{SearchText}")]
        public async Task<ActionResult<APIResponse>> SearchDepartment(string SearchBy, string SearchText)
        {
            if (SearchBy == null || SearchText == null)
                return _apiResponse.ResponseToClient(false, HttpStatusCode.BadRequest, "Fields cannot be empty", null, string.Empty);

            List<SchoolAdmin> result = null;
            string[] searchItems = new[] { "Name", "Email" };

            // Normalize SearchBy to correct casing then remove the looping
            //SearchBy = searchItems.FirstOrDefault(s => s.Equals(SearchBy, StringComparison.OrdinalIgnoreCase));

            //or 

            // Capitalize first letter to match property names
            SearchBy = char.ToUpper(SearchBy[0]) + SearchBy.Substring(1);

            for (var i = 0; i < searchItems.Length; i++)
            {
                if (SearchBy == searchItems[i])
                    result = await _collegeRepository.GetAllFilterAsync(x => EF.Property<string>(x, SearchBy).ToLower().Contains(SearchText));
            }

            if (result == null)
                return _apiResponse.ResponseToClient(false, HttpStatusCode.NotFound, "Could not find any data related", null, string.Empty);

            var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully retrieved related data", result, string.Empty);
            return Ok(response);
        }

        [HttpPost("upload-excel")]
        public async Task<ActionResult<APIResponse>> UploadDepartment(IFormFile file)
        {
            var schoolAdminDTO = await _uploadExcel.ImportExcelAsync(file);
            if (schoolAdminDTO == null || schoolAdminDTO.Count == 0)
                return _apiResponse.ResponseToClient(false, HttpStatusCode.BadRequest, "No data found in the file", null, string.Empty);

            var department = _mapper.Map<List<SchoolAdmin>>(schoolAdminDTO);
            if (department == null)
                return _apiResponse.ResponseToClient(false, HttpStatusCode.BadRequest, "Could not convert data", null, string.Empty);

            await _collegeRepository.CreateRangeAsync(department);
            var result = _apiResponse.ResponseToClient(true, HttpStatusCode.Created, "Successfully added all datas", null, string.Empty);

            return Ok(result);
        }



        [HttpPost("create")]
        public async Task<ActionResult<APIResponse>> NewSchoolAdmin([FromBody] SchoolAdminDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest("Empty Request");

                if (!ModelState.IsValid)
                    return BadRequest();

                SchoolAdmin admin = _mapper.Map<SchoolAdmin>(dto);
                admin.IsActive = true;
                admin.AssignedDate = DateTime.Now;

                dto.Id = admin.Id;
                var result = await _collegeRepository.CreateAsync(admin);

                APIResponse response = _apiResponse.ResponseToClient(true, HttpStatusCode.Created, "Successfully added data", dto, string.Empty);
                return CreatedAtAction("GetSchoolAdminById", new { Id = dto.Id }, response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error adding data", null, ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<APIResponse>> UpdateSchoolAdminn([FromBody] SchoolAdminDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest("Empty Request");

                var existingResult = await _collegeRepository.GetAsync(c => c.Id == dto.Id, null, true);
                if (existingResult == null)
                    return NotFound("Data could not be found");

                existingResult = _mapper.Map<SchoolAdmin>(dto);
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
        public async Task<ActionResult<APIResponse>> DeleteSchoolAdmin(int Id)
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
