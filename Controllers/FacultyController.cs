using CollegeManagement.Data.Model;
using CollegeManagement.Data;
using CollegeManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using CollegeManagement.Data.IRepository;
using CollegeManagement.Data.IServices;
using Microsoft.AspNetCore.Authorization;

namespace CollegeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class FacultyController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly APIResponse _apiResponse;
        private readonly IUploadExcel<FacultyDTO> _uploadExcel;
        public FacultyController(IUnitOfWork unitOfWork, IMapper mapper, IUploadExcel<FacultyDTO> uploadExcel)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _apiResponse = new APIResponse();
            _uploadExcel = uploadExcel;
        }

        [HttpGet]
        
        public async Task<ActionResult<APIResponse>> GetAllFaculties()
        {
            try
            {
                var result = await _unitOfWork.FacultyRepositoryInterface.GetAllAsync(new List<string> { "School", "FacultyHeads", "Departments"});
                if (result == null)
                    return NotFound("Looks like there's not data yet");

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully retrieved data", result, string.Empty);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(true, HttpStatusCode.InternalServerError, "Error retrieving data", null, ex.Message);
            }
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<APIResponse>> GetFacultyById(int Id)
        {
            try
            {
                if (Id == 0)
                    return BadRequest("Id cannot be less than or equal to zero");

                var result = await _unitOfWork.FacultyRepositoryInterface.GetAsync(f => f.Id == Id, new List<string> { "School", "FacultyHeads", "Departments" }, false);
                if (result == null)
                    return NotFound("Looks like there's no data yet");

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully retrieved data", result, string.Empty);

                return Ok(_apiResponse);

            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error retrieving data", null, ex.Message);
            }
        }

        [HttpGet("getAllFacultyBySchool/{schoolId}")]
        public async Task<ActionResult<APIResponse>> GetAllFacultyBySchoolId(int schoolId)
        {
            try
            {
                if (schoolId == 0)
                    return BadRequest("Empty Request");

                List<Faculty> result = await _unitOfWork.FacultyRepositoryInterface.GetAllFilterAsync(br => br.SchoolId == schoolId, null, new List<string> { "School", "FacultyHeads", "Departments", "Courses" });
                if (result == null)
                    return _apiResponse.ResponseToClient(true, HttpStatusCode.NotFound, "No data found", result, string.Empty);

                //Faculty dto = _mapper.Map<Faculty>(result);
                return _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully retrieved data", result, string.Empty);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error retrieving data", null, ex.Message);
            }

        }

        [HttpGet("search/{SearchBy}/{SearchText}")]
        public async Task<ActionResult<APIResponse>> SearchStudent(string SearchBy, string SearchText)
        {
            if (SearchBy == null || SearchText == null)
                return _apiResponse.ResponseToClient(false, HttpStatusCode.BadRequest, "Fields cannot be empty", null, string.Empty);

            List<Faculty> result = null;
            string[] searchItems = new[] { "Name", "Description", "SchoolId", "FacultyCode" };

            // Normalize SearchBy to correct casing then remove the looping
            //SearchBy = searchItems.FirstOrDefault(s => s.Equals(SearchBy, StringComparison.OrdinalIgnoreCase));

            //or 

            // Capitalize first letter to match property names
            SearchBy = char.ToUpper(SearchBy[0]) + SearchBy.Substring(1);

            for (var i = 0; i < searchItems.Length; i++)
            {
                if (SearchBy == searchItems[i])
                    result = await _unitOfWork.FacultyRepositoryInterface.GetAllFilterAsync(x => EF.Property<string>(x, SearchBy).ToLower().Contains(SearchText));
            }

            if (result == null)
                return _apiResponse.ResponseToClient(false, HttpStatusCode.NotFound, "Could not find any data related", null, string.Empty);

            var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully retrieved related data", result, string.Empty);
            return Ok(response);
        }

        [HttpGet("faculty/head/{Id}")]
        public async Task<ActionResult<APIResponse>> GetFacultyHeadById(int Id)
        {
            try
            {
                if (Id <= 0)
                    return _apiResponse.ResponseToClient(false, HttpStatusCode.BadRequest, "Id cannot be less than or equal to zero", null, string.Empty);

                var result = await _unitOfWork.FacultyRepositoryInterface.GetFacultyHeadById(Id);

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully retrieved data", result, string.Empty);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error retrieving data", null, ex.Message);
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<APIResponse>> NewFaculty([FromBody] FacultyDTO dto)
        {
            try
            {
                if (dto == null)
                    return _apiResponse.ResponseToClient(true, HttpStatusCode.BadRequest, "Fields cannot be null", dto, string.Empty);

                if (!ModelState.IsValid)
                    return _apiResponse.ResponseToClient(true, HttpStatusCode.BadRequest, "Fields are invalid", dto, string.Empty);

                Faculty fac = _mapper.Map<Faculty>(dto);
                dto.Id = fac.Id;
                await _unitOfWork.FacultyRepositoryInterface.CreateAsync(fac);
                _unitOfWork.Save();

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.Created, "Successfully added data", dto, string.Empty);

                return CreatedAtAction("GetFacultyById", new { Id = dto.Id }, response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error adding data", null, ex.Message);
            }
        }

        [HttpPost("upload-excel")]
        public async Task<ActionResult<APIResponse>> UploadFaculty(IFormFile file)
        {
            var facultyDto = await _uploadExcel.ImportExcelAsync(file);
            if (facultyDto == null || facultyDto.Count == 0)
                return _apiResponse.ResponseToClient(false, HttpStatusCode.BadRequest, "No data found in the file", null, string.Empty);

            var faculty = _mapper.Map<List<Faculty>>(facultyDto);
            await _unitOfWork.FacultyRepositoryInterface.CreateRangeAsync(faculty);

            var result = _apiResponse.ResponseToClient(true, HttpStatusCode.Created, "Successfully added all datas", null, string.Empty);

            return Ok(result);
        }

        [HttpPost("faculty/head")]
        public async Task<ActionResult<APIResponse>> CreateDepartmentalHead([FromBody] FacultyHeadDTO dto)
        {
            try
            {
                if (dto == null)
                    return _apiResponse.ResponseToClient(false, HttpStatusCode.BadRequest, "No data sent", null, string.Empty);
                if (!ModelState.IsValid)
                    return _apiResponse.ResponseToClient(false, HttpStatusCode.BadRequest, "Invalid field", null, string.Empty);

                var result = await _unitOfWork.FacultyRepositoryInterface.CreateFacultyHead(dto);
                _unitOfWork.Save();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error adding data", null, ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<APIResponse>> UpdateFaculty([FromBody] FacultyDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest("Fields cannot be null");

                var existingResult = await _unitOfWork.FacultyRepositoryInterface.GetAsync(dh => dh.Id == dto.Id, null, true);

                existingResult = _mapper.Map<Faculty>(dto);
                dto.Id = existingResult.Id;
                await _unitOfWork.FacultyRepositoryInterface.UpdateAsync(existingResult);
                _unitOfWork.Save();
                dto.Id = existingResult.Id;

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully updated data", null, string.Empty);


                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error updating data", null, ex.Message);
            }
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult<APIResponse>> DeleteFaculty(int Id)
        {
            try
            {
                if (Id <= 0)
                    return BadRequest("Id cannot be less than or equal to zero");

                var existingResult = await _unitOfWork.FacultyRepositoryInterface.GetAsync(x => x.Id == Id);
                if (existingResult == null)
                    return NotFound("No Department found with the provided Id");

                
                await _unitOfWork.FacultyRepositoryInterface.DeleteAsync(existingResult);
                _unitOfWork.Save();

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully deleted data", null, string.Empty);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error deleting data", null, ex.Message);
            }

        }
    }
}
