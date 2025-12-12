using AutoMapper;
using CollegeManagement.Data.Identity;
using CollegeManagement.Data.IRepository;
using CollegeManagement.Data.IServices;
using CollegeManagement.Data.Model;
using CollegeManagement.Data.Repository;
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
    [Authorize(Roles = "SuperAdmin")]
    public class RoleController : ControllerBase
    {
        private readonly ICollegeRepository<Role> _roleRepository;
        private readonly IMapper _mapper;
        private readonly APIResponse _apiResponse;
        private readonly IUploadExcel<RoleDTO> _uploadExcel;
        public RoleController(ICollegeRepository<Role> collegeRepository, IMapper mapper, IUploadExcel<RoleDTO> uploadExcel)
        {
            _roleRepository = collegeRepository;
            _mapper = mapper;
            _apiResponse = new APIResponse();
            _uploadExcel = uploadExcel;
        }

        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetAllRoles()
        {
            try
            {
                var allRoles = await _roleRepository.GetAllAsync();
                if (allRoles == null)
                    return _apiResponse.ResponseToClient(false, HttpStatusCode.NotFound, "No roles found", null, string.Empty);

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Roles retrieved successfully", allRoles, string.Empty);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error retrieving roles", null, ex.Message);
            }
        }
               

        [HttpGet("GetRoleById/{Id}")]
        public async Task<ActionResult<APIResponse>> GetRoleById(int Id)
        {
            try
            {
                if (Id <= 0 )
                    return _apiResponse.ResponseToClient(false, HttpStatusCode.BadRequest, "Invalid role Id", null, string.Empty);

                var existingResult = await _roleRepository.GetAsync(x => x.Id == Id);
                if (existingResult == null)
                    return _apiResponse.ResponseToClient(false, HttpStatusCode.NotFound, "No role found", null, string.Empty);

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Roles retrieved successfully", existingResult, string.Empty);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error retrieving roles", null, ex.Message);
            }
        }

        [HttpGet("GetRoleByName/{Name}")]
        public async Task<ActionResult<APIResponse>> GetRoleByName(string Name)
        {
            try
            {
                if (string.IsNullOrEmpty(Name))
                    return _apiResponse.ResponseToClient(false, HttpStatusCode.BadRequest, "Invalid role Id", null, string.Empty);

                var existingResult = await _roleRepository.GetAsync(x => x.RoleName == Name);
                if (existingResult == null)
                    return _apiResponse.ResponseToClient(false, HttpStatusCode.NotFound, "No role found", null, string.Empty);

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Roles retrieved successfully", existingResult, string.Empty);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error retrieving roles", null, ex.Message);
            }
        }

        [HttpGet("search/{SearchBy}/{SearchText}")]
        public async Task<ActionResult<APIResponse>> SearchDepartment(string SearchBy, string SearchText)
        {
            if (SearchBy == null || SearchText == null)
                return _apiResponse.ResponseToClient(false, HttpStatusCode.BadRequest, "Fields cannot be empty", null, string.Empty);

            List<Role> result = null;
            string[] searchItems = new[] { "RoleName" };

            // Normalize SearchBy to correct casing then remove the looping
            //SearchBy = searchItems.FirstOrDefault(s => s.Equals(SearchBy, StringComparison.OrdinalIgnoreCase));

            //or 

            // Capitalize first letter to match property names
            SearchBy = char.ToUpper(SearchBy[0]) + SearchBy.Substring(1);

            for (var i = 0; i < searchItems.Length; i++)
            {
                if (SearchBy == searchItems[i])
                    result = await _roleRepository.GetAllFilterAsync(x => EF.Property<string>(x, SearchBy).ToLower().Contains(SearchText));
            }

            if (result == null)
                return _apiResponse.ResponseToClient(false, HttpStatusCode.NotFound, "Could not find any data related", null, string.Empty);

            var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully retrieved related data", result, string.Empty);
            return Ok(response);
        }

        [HttpPost("upload-excel")]
        public async Task<ActionResult<APIResponse>> UploadDepartment(IFormFile file)
        {
            var roleDto = await _uploadExcel.ImportExcelAsync(file);
            if (roleDto == null || roleDto.Count == 0)
                return _apiResponse.ResponseToClient(false, HttpStatusCode.BadRequest, "No data found in the file", null, string.Empty);

            var role = _mapper.Map<List<Role>>(roleDto);
            if (role == null)
                return _apiResponse.ResponseToClient(false, HttpStatusCode.BadRequest, "Could not convert data", null, string.Empty);

            await _roleRepository.CreateRangeAsync(role);

            var result = _apiResponse.ResponseToClient(true, HttpStatusCode.Created, "Successfully added all roles", null, string.Empty);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<APIResponse>> CreateNewRole([FromBody] RoleDTO dto)
        {
            try
            {
                if (dto == null)
                    return _apiResponse.ResponseToClient(false, HttpStatusCode.BadRequest, "Invalid role data", null, string.Empty);

                var mapRole = _mapper.Map<Role>(dto);
                mapRole.IsDeleted = false;
                mapRole.CreatedDate = DateTime.Now;
                mapRole.ModifiedDate = DateTime.Now;
                dto.Id = mapRole.Id;

                await _roleRepository.CreateAsync(mapRole);
                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.Created, "Role created successfully", null, string.Empty);
                return CreatedAtAction("GetRoleById", new {Id = dto.Id}, response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error creating role", null, ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<APIResponse>> UpdateRole([FromBody] RoleDTO dto)
        {
            try
            {
                if (dto == null || dto.Id <= 0)
                    return _apiResponse.ResponseToClient(false, HttpStatusCode.BadRequest, "Invalid role data", null, string.Empty);

                var existingResult = await _roleRepository.GetAsync(x => x.Id == dto.Id, null, true);
                if (existingResult == null)
                    return _apiResponse.ResponseToClient(false, HttpStatusCode.NotFound, "No role found", null, string.Empty);

                var mapRole = _mapper.Map<Role>(dto);
                mapRole.IsDeleted = false;
                mapRole.ModifiedDate = DateTime.Now;
                dto.Id = mapRole.Id;

                await _roleRepository.UpdateAsync(mapRole);
                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Role updated successfully", null, string.Empty);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error updating role", null, ex.Message);
            }
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult<APIResponse>> DeleteRole(int Id)
        {
            try
            {
                if (Id == 0)
                    return _apiResponse.ResponseToClient(false, HttpStatusCode.BadRequest, "Invalid role data", null, string.Empty);

                var existingResult = await _roleRepository.GetAsync(x => x.Id == Id, null, true);
                if (existingResult == null)
                    return _apiResponse.ResponseToClient(false, HttpStatusCode.NotFound, "No role found", null, string.Empty);

                //var mapRole = _mapper.Map<Role>(dto);
                existingResult.IsDeleted = true;

                await _roleRepository.DeleteAsync(existingResult);
                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Role deleted successfully", null, string.Empty);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error deleting role", null, ex.Message);
            }
        }



    }
}
