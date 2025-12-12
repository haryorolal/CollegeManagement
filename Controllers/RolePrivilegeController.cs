using AutoMapper;
using CollegeManagement.Data.Identity;
using CollegeManagement.Data.IRepository;
using CollegeManagement.Data.IServices;
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
    public class RolePrivilegeController : ControllerBase
    {
        private readonly ICollegeRepository<RolePrivilege> _roleRepository;
        private readonly IMapper _mapper;
        private readonly APIResponse _apiResponse;
        
        public RolePrivilegeController(ICollegeRepository<RolePrivilege> collegeRepository, IMapper mapper)
        {
            _roleRepository = collegeRepository;
            _mapper = mapper;
            _apiResponse = new APIResponse();
            
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult<APIResponse>> GetAllRolePrivileges()
        {
            try
            {
                var allRolePrivileges = await _roleRepository.GetAllAsync();
                if (allRolePrivileges == null)
                    return _apiResponse.ResponseToClient(false, HttpStatusCode.NotFound, "No role privileges found", null, string.Empty);

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Role privileges retrieved successfully", allRolePrivileges, string.Empty);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error retrieving role privileges", null, ex.Message);
            }
        }


        [HttpGet("GetRolePrivilegeById/{Id}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult<APIResponse>> GetRolePrivilegeById(int Id)
        {
            try
            {
                if (Id <= 0)
                    return _apiResponse.ResponseToClient(false, HttpStatusCode.BadRequest, "Invalid role privilege Id", null, string.Empty);

                var existingResult = await _roleRepository.GetAsync(x => x.Id == Id);
                if (existingResult == null)
                    return _apiResponse.ResponseToClient(false, HttpStatusCode.NotFound, "No role privilege found", null, string.Empty);

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Role privileges retrieved successfully", existingResult, string.Empty);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error retrieving role privileges", null, ex.Message);
            }
        }


        [HttpGet("GetRolePrivilegeByName/{Name}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult<APIResponse>> GetRolePrivilegeByName(string Name)
        {
            try
            {
                if (string.IsNullOrEmpty(Name))
                    return _apiResponse.ResponseToClient(false, HttpStatusCode.BadRequest, "Invalid role privilege Name", null, string.Empty);

                var existingResult = await _roleRepository.GetAsync(x => x.RolePrivilegeName == Name);
                if (existingResult == null)
                    return _apiResponse.ResponseToClient(false, HttpStatusCode.NotFound, "No role found", null, string.Empty);

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Role privileges retrieved successfully", existingResult, string.Empty);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error retrieving role privileges", null, ex.Message);
            }
        }

        [HttpGet("GetAllRolePrivilegesByRoleId/{RoleId}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult<APIResponse>> GetAllRolePrivilegesByRoleId(int RoleId)
        {
            try
            {
                if (RoleId <= 0)
                    return _apiResponse.ResponseToClient(false, HttpStatusCode.BadRequest, "Invalid role privilege Id", null, string.Empty);

                var existingResult = await _roleRepository.GetAllFilterAsync(x => x.RoleId == RoleId);
                if (existingResult == null)
                    return _apiResponse.ResponseToClient(false, HttpStatusCode.NotFound, "No role privilege found", null, string.Empty);

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Role privileges retrieved successfully", existingResult, string.Empty);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error retrieving role privileges", null, ex.Message);
            }
        }

       

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult<APIResponse>> CreateNewRolePrivilege([FromBody] RolePrivilegeDTO dto)
        {
            try
            {
                if (dto == null)
                    return _apiResponse.ResponseToClient(false, HttpStatusCode.BadRequest, "Invalid role privilege data", null, string.Empty);

                var mapRole = _mapper.Map<RolePrivilege>(dto);
                mapRole.IsDeleted = false;
                mapRole.CreatedDate = DateTime.Now;
                mapRole.ModifiedDate = DateTime.Now;
                dto.Id = mapRole.Id;

                await _roleRepository.CreateAsync(mapRole);
                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.Created, "Role privilege assigned successfully", null, string.Empty);
                return CreatedAtAction("GetRolePrivilegeById", new { Id = dto.Id }, response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error creating role privilege", null, ex.Message);
            }
        }

        [HttpPut]
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<ActionResult<APIResponse>> UpdateRolePrivilege([FromBody] RolePrivilegeDTO dto)
        {
            try
            {
                if (dto == null || dto.Id <= 0)
                    return _apiResponse.ResponseToClient(false, HttpStatusCode.BadRequest, "Invalid roleprivilege data", null, string.Empty);

                var existingResult = await _roleRepository.GetAsync(x => x.Id == dto.Id, null, true);
                if (existingResult == null)
                    return _apiResponse.ResponseToClient(false, HttpStatusCode.NotFound, "No role privilege found", null, string.Empty);

                var mapRole = _mapper.Map<RolePrivilege>(dto);
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
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult<APIResponse>> DeleteRolePrivilege(int Id)
        {
            try
            {
                if (Id == 0)
                    return _apiResponse.ResponseToClient(false, HttpStatusCode.BadRequest, "Invalid roleprivilege data", null, string.Empty);

                var existingResult = await _roleRepository.GetAsync(x => x.Id == Id, null, false);
                if (existingResult == null)
                    return _apiResponse.ResponseToClient(false, HttpStatusCode.NotFound, "No role privilege found", null, string.Empty);

                existingResult.IsDeleted = true;
                await _roleRepository.DeleteAsync(existingResult);

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Role privilege removed successfully", null, string.Empty);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error deleting roleprivilege", null, ex.Message);
            }
        }

    }
}
