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
    public class DepartmentController : ControllerBase
    {
        private readonly ICollegeRepository<Department> _departmentRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly APIResponse _apiResponse;
        private readonly IUploadExcel<DepartmentDTO> _uploadExcel;
        private readonly 
        public DepartmentController(ICollegeRepository<Department> departmentRepo, IUnitOfWork unitOfWork, IMapper mapper, IUploadExcel<DepartmentDTO> uploadExcel)
        {
            _unitOfWork = unitOfWork;
            _departmentRepo = departmentRepo;
            _mapper = mapper;
            _uploadExcel = uploadExcel;
            _apiResponse = new APIResponse();
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult<APIResponse>> GetAllDepartments()
        {
            try
            {
                var result = await _unitOfWork.DepartmentRepositoryInterface.GetAllAsync(new List<string> { "DepartmentHeads",  "Courses", "Exams", "Students", "Staffs" });
                if (result == null)
                    return _apiResponse.ResponseToClient(false, HttpStatusCode.NotFound, "Looks like there's not data yet", null, string.Empty);

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
        public async Task<ActionResult<APIResponse>> GetDepartmentsById(int Id)
        {
            try
            {
                var result = await _unitOfWork.DepartmentRepositoryInterface.GetAsync(d => d.Id == Id, new List<string> { "DepartmentHeads", "Students", "Courses", "Exams", "Staffs" }, false);
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


        [HttpGet("getAllDepartmentBy/schoolId/facultyId/{schoolId}/{facultyId}")]
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<ActionResult<APIResponse>> GetAllDepartmentsBySchooIdAndFacultyId(int schoolId, int facultyId)
        {
            try
            {
                List<Department> result = await _unitOfWork.DepartmentRepositoryInterface.GetAllFilterAsync(d => d.SchoolId == schoolId && d.FacultyId == facultyId, null, new List<string> { "DepartmentHeads", "Students", "Courses", "Exams", "Staffs" }, false);
                if (result == null)
                    return _apiResponse.ResponseToClient(false, HttpStatusCode.NotFound, "No data found", null, string.Empty);

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully retrieved data", result, string.Empty);
                return Ok(response);

            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error retrieving data", null, ex.Message);
            }
        }

        [HttpGet("getAllDepartmentBy/facultyId/{facultyId}")]
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<ActionResult<APIResponse>> GetAllDepartmentsByFacultyId(int facultyId)
        {
            try
            {
                List<Department> result = await _unitOfWork.DepartmentRepositoryInterface.GetAllFilterAsync(d => d.FacultyId == facultyId, null, new List<string> { "DepartmentHeads", "Students", "Courses", "Exams", "Staffs" }, false);
                if (result == null)
                    return _apiResponse.ResponseToClient(false, HttpStatusCode.NotFound, "No data found", null, string.Empty);

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully retrieved data", result, string.Empty);
                return Ok(response);

            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error retrieving data", null, ex.Message);
            }
        }

        [HttpGet("getAllDepartmentBySchool/{schoolId}")]
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<ActionResult<APIResponse>> GetAllDepartmentsBySchoolId(int schoolId)
        {
            try
            {
                List<Department> result = await _unitOfWork.DepartmentRepositoryInterface.GetAllFilterAsync(d => d.SchoolId == schoolId, null, new List<string> { "DepartmentHeads", "Students", "Courses", "Exams", "Staffs" }, false);
                if (result == null)
                    return _apiResponse.ResponseToClient(false, HttpStatusCode.NotFound, "No data found", null, string.Empty);

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully retrieved data", result, string.Empty);
                return Ok(response);

            }
            catch(Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error retrieving data", null, ex.Message);
            }
        }

        [HttpGet("head/{Id}")]
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<ActionResult<APIResponse>> GetDepartmentalHeadById(int Id)
        {
            try
            {
                if (Id <= 0)
                    return _apiResponse.ResponseToClient(false, HttpStatusCode.BadRequest, "Id cannot be less than or equal to zero", null, string.Empty);

                var result = await _unitOfWork.DepartmentRepositoryInterface.GetDepartmentalHeadById(Id);              

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully retrieved data", result, string.Empty);
                return Ok(response);
            }
            catch(Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error retrieving data", null, ex.Message);
            }
        }

        [HttpGet("search/{SearchBy}/{SearchText}")]
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<ActionResult<APIResponse>> SearchDepartment(string SearchBy, string SearchText)
        {
            if (SearchBy == null || SearchText == null)
                return _apiResponse.ResponseToClient(false, HttpStatusCode.BadRequest, "Fields cannot be empty", null, string.Empty);

            List<Department> result = null;
            string[] searchItems = new[] { "Name", "DepartmentCode" };

            // Normalize SearchBy to correct casing then remove the looping
            //SearchBy = searchItems.FirstOrDefault(s => s.Equals(SearchBy, StringComparison.OrdinalIgnoreCase));

            //or 

            // Capitalize first letter to match property names
            SearchBy = char.ToUpper(SearchBy[0]) + SearchBy.Substring(1);

            for (var i = 0; i < searchItems.Length; i++)
            {
                if (SearchBy == searchItems[i])
                    result = await _unitOfWork.DepartmentRepositoryInterface.GetAllFilterAsync(x => EF.Property<string>(x, SearchBy).ToLower().Contains(SearchText));
            }

            if (result == null)
                return _apiResponse.ResponseToClient(false, HttpStatusCode.NotFound, "Could not find any data related", null, string.Empty);

            var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully retrieved related data", result, string.Empty);
            return Ok(response);
        }

        [HttpPost("upload-excel")]
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<ActionResult<APIResponse>> UploadDepartment(IFormFile file)
        {
            await _unitOfWork.BeginTransactionAsync();
            try 
            { 
                var departmentDto = await _uploadExcel.ImportExcelAsync(file);
                if (departmentDto == null || departmentDto.Count == 0)
                    return _apiResponse.ResponseToClient(false, HttpStatusCode.BadRequest, "No data found in the file", null, string.Empty);

                var department = _mapper.Map<List<Department>>(departmentDto);
                if (department == null)
                    return _apiResponse.ResponseToClient(false, HttpStatusCode.BadRequest, "Could not convert data", null, string.Empty);

                await _unitOfWork.DepartmentRepositoryInterface.CreateRangeAsync(department);
                _unitOfWork.Save();
                await _unitOfWork.CommitAsync();

                var result = _apiResponse.ResponseToClient(true, HttpStatusCode.Created, "Successfully added all datas", null, string.Empty);
                return Ok(result);

            }catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error adding all datas", null, ex.Message);
            }

        }

        [HttpPost("create")]
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<ActionResult<APIResponse>> NewDepartment([FromBody] DepartmentDTO dto)
        {
            try
            {
                if (dto == null)
                    return _apiResponse.ResponseToClient(true, HttpStatusCode.BadRequest, "Fields cannot be null", dto, string.Empty);

                if (!ModelState.IsValid)
                    return _apiResponse.ResponseToClient(true, HttpStatusCode.BadRequest, "Fields are invalid", dto, string.Empty);

                Department dept = _mapper.Map<Department>(dto);
                dto.Id = dept.Id;
                await _unitOfWork.DepartmentRepositoryInterface.CreateAsync(dept);
                _unitOfWork.Save();

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully added data", dto, string.Empty);

                return CreatedAtAction("GetDepartmentsById", new { Id = dto.Id }, response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error adding data", null, ex.Message);
            }
        }        

        [HttpPost("head")]
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<ActionResult<APIResponse>> CreateDepartmentalHead([FromBody] DepartmentHeadDTO dto)
        {
            try
            {
                if (dto == null)
                    return _apiResponse.ResponseToClient(false, HttpStatusCode.BadRequest, "No data sent", null, string.Empty);
                if (!ModelState.IsValid)
                    return _apiResponse.ResponseToClient(false, HttpStatusCode.BadRequest, "Invalid field", null, string.Empty);

                var result = await _unitOfWork.DepartmentRepositoryInterface.CreateDepartmentalHead(dto);
                _unitOfWork.Save();
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error adding data", null, ex.Message);
            }
        }


        [HttpPut]
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<ActionResult<APIResponse>> UpdateDepartment([FromBody] DepartmentDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest("Fields cannot be null");
                if (!ModelState.IsValid)
                    return BadRequest();

                var existingResult = await _unitOfWork.DepartmentRepositoryInterface.GetAsync(c => c.Id == dto.Id, null, true);
                if (existingResult == null)
                    return NotFound("No course found with the provided Id");

                existingResult = _mapper.Map<Department>(dto);
                dto.Id = existingResult.Id;
                await _unitOfWork.DepartmentRepositoryInterface.UpdateAsync(existingResult);
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
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<ActionResult<APIResponse>> DeleteDepartment(int Id)
        {
            try
            {
                if (Id == 0)
                    return BadRequest("Id cannot be less than or equal to zero");

                var existingResult = await _unitOfWork.DepartmentRepositoryInterface.GetAsync(x => x.Id == Id);
                if (existingResult == null)
                    return NotFound("No course found with the provided Id");

                await _unitOfWork.DepartmentRepositoryInterface.DeleteAsync(existingResult);
                _unitOfWork.Save();

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
