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
    public class UserTypeController : ControllerBase
    {
        private readonly ICollegeRepository<UserType> _roleRepository;
        private readonly IMapper _mapper;
        private readonly APIResponse _apiResponse;
        private readonly LibraryDbContext _libraryDbContext;
        private readonly IUploadExcel<UserTypeDTO> _uploadExcel;
        public UserTypeController(ICollegeRepository<UserType> collegeRepository, IMapper mapper, LibraryDbContext libraryDbContext, IUploadExcel<UserTypeDTO> uploadExcel)
        {
            _roleRepository = collegeRepository;
            _mapper = mapper;
            _apiResponse = new APIResponse();
            _libraryDbContext = libraryDbContext;
            _uploadExcel = uploadExcel;
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<ActionResult<APIResponse>> GetAllUserTypes()
        {
            try
            {
                var roles = _libraryDbContext.Roles.FirstOrDefault();
                //var allRole = await _roleRepository.GetAllFilterAsync(x => x.Name.Substring(6, 0) );
                var allRoles = await _roleRepository.GetAllAsync();
                if (allRoles == null)
                    return _apiResponse.ResponseToClient(false, HttpStatusCode.NotFound, "Not found", null, string.Empty);
                

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Retrieved successfully", allRoles, string.Empty);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error retrieving", null, ex.Message);
            }
        }


        [HttpGet("{Id}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult<APIResponse>> GetUserTypeById(int Id)
        {
            try
            {
                if (Id <= 0)
                    return _apiResponse.ResponseToClient(false, HttpStatusCode.BadRequest, "Invalid Id", null, string.Empty);

                var existingResult = await _roleRepository.GetAsync(x => x.Id == Id);
                if (existingResult == null)
                    return _apiResponse.ResponseToClient(false, HttpStatusCode.NotFound, "Not found", null, string.Empty);

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Retrieved successfully", existingResult, string.Empty);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error retrieving", null, ex.Message);
            }
        }

        [HttpGet("userTypeByname/{Name}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult<APIResponse>> GetUserTypeByName(string Name)
        {
            try
            {
                if (string.IsNullOrEmpty(Name))
                    return _apiResponse.ResponseToClient(false, HttpStatusCode.BadRequest, "Invalid Id", null, string.Empty);

                var existingResult = await _roleRepository.GetAsync(x => x.Name == Name);
                if (existingResult == null)
                    return _apiResponse.ResponseToClient(false, HttpStatusCode.NotFound, "Not found", null, string.Empty);

                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Retrieved successfully", existingResult, string.Empty);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error retrieving", null, ex.Message);
            }
        }

        [HttpGet("search/{SearchBy}/{SearchText}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult<APIResponse>> SearchUserType(string SearchBy, string SearchText)
        {
            if (SearchBy == null || SearchText == null)
                return _apiResponse.ResponseToClient(false, HttpStatusCode.BadRequest, "Fields cannot be empty", null, string.Empty);

            List<UserType> result = null;
            string[] searchItems = new[] { "Code", "Name" };

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
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult<APIResponse>> UploadUserType(IFormFile file)
        {
            var dto = await _uploadExcel.ImportExcelAsync(file);
            if (dto == null || dto.Count == 0)
                return _apiResponse.ResponseToClient(false, HttpStatusCode.BadRequest, "No data found in the file", null, string.Empty);

            var convertedResult = _mapper.Map<List<UserType>>(dto);
            if (convertedResult == null)
                return _apiResponse.ResponseToClient(false, HttpStatusCode.BadRequest, "Could not convert data", null, string.Empty);

            await _roleRepository.CreateRangeAsync(convertedResult);

            var result = _apiResponse.ResponseToClient(true, HttpStatusCode.Created, "Successfully added all roles", null, string.Empty);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult<APIResponse>> CreateNewUserType([FromBody] UserTypeDTO dto)
        {
            try
            {
                if (dto == null)
                    return _apiResponse.ResponseToClient(false, HttpStatusCode.BadRequest, "Invalid data", null, string.Empty);

                var mapRole = _mapper.Map<UserType>(dto);
                dto.Id = mapRole.Id;

                await _roleRepository.CreateAsync(mapRole);
                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.Created, "Created successfully", null, string.Empty);
                return CreatedAtAction("GetUserTypeById", new { Id = dto.Id }, response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Error creating", null, ex.Message);
            }
        }

        [HttpPut]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult<APIResponse>> UpdateUserType([FromBody] UserTypeDTO dto)
        {
            try
            {
                if (dto == null || dto.Id <= 0)
                    return _apiResponse.ResponseToClient(false, HttpStatusCode.BadRequest, "Invalid data", null, string.Empty);

                var existingResult = await _roleRepository.GetAsync(x => x.Id == dto.Id, null, true);
                if (existingResult == null)
                    return _apiResponse.ResponseToClient(false, HttpStatusCode.NotFound, "Not found", null, string.Empty);

                var mapRole = _mapper.Map<UserType>(dto);
                dto.Id = mapRole.Id;

                await _roleRepository.UpdateAsync(mapRole);
                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Updated successfully", null, string.Empty);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Something went wrong", null, ex.Message);
            }
        }

        [HttpDelete("{Id}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult<APIResponse>> DeleteUserType(int Id)
        {
            try
            {
                if (Id == 0)
                    return _apiResponse.ResponseToClient(false, HttpStatusCode.BadRequest, "Invalid  data", null, string.Empty);

                var existingResult = await _roleRepository.GetAsync(x => x.Id == Id, null, true);
                if (existingResult == null)
                    return _apiResponse.ResponseToClient(false, HttpStatusCode.NotFound, "Not found", null, string.Empty);

                await _roleRepository.DeleteAsync(existingResult);
                var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Deleted successfully", null, string.Empty);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Something went wrong", null, ex.Message);
            }
        }



    }
}
