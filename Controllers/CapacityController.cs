using AutoMapper;
using CollegeManagement.Data.IRepository;
using CollegeManagement.Data.Model;
using CollegeManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CollegeManagement.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    //public class CapacityController : ControllerBase
    //{
    //    private readonly ICollegeRepository<Capacity> _CapacityRepo;
    //    private readonly IMapper _mapper;
    //    private readonly APIResponse _apiResponse;

    //    public CapacityController(ICollegeRepository<Capacity> collegeRepository, IMapper mapper)
    //    {
    //        _CapacityRepo = collegeRepository;
    //        _mapper = mapper;
    //        _apiResponse = new APIResponse();
    //    }

        //[HttpGet]
        //public async Task<ActionResult<APIResponse>> GetAllCapacities()
        //{
        //    try
        //    {
        //        var result = await _CapacityRepo.GetAllAsync();
        //        if (result.Count == 0)
        //            return _apiResponse.ResponseToClient(false, HttpStatusCode.NotFound, "Could not find any capacity", null, string.Empty);

        //        var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully retrieved all datas", result, string.Empty);
        //        return Ok(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Something went wrong", null, ex.Message);
        //    }
        //}

        //[HttpGet("{Id}")]
        //public async Task<ActionResult<APIResponse>> GetCapacityById(int Id)
        //{
        //    try
        //    {
        //        var result = await _CapacityRepo.GetAsync(x => x.Id == Id);
        //        if (result == null)
        //            return _apiResponse.ResponseToClient(false, HttpStatusCode.NotFound, "Could not find capacity", null, string.Empty);

        //        var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully retrieved data", result, string.Empty);
        //        return Ok(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Something went wrong", null, ex.Message);
        //    }
        //}

        //[HttpPost]
        //public async Task<ActionResult<APIResponse>> NewCapacity([FromBody] Capacity dto)
        //{
        //    try
        //    {
        //        if (dto == null)
        //            return _apiResponse.ResponseToClient(false, HttpStatusCode.BadRequest, "No data sent", null, string.Empty);

        //        if (!ModelState.IsValid)
        //            return _apiResponse.ResponseToClient(false, HttpStatusCode.BadRequest, "Fields are not valid", null, string.Empty);

        //        var mappedCapacity = _mapper.Map<Capacity>(dto);
        //        dto.Id = mappedCapacity.Id;
        //        await _CapacityRepo.CreateAsync(mappedCapacity);

        //        var response = _apiResponse.ResponseToClient(true, HttpStatusCode.Created, "Successfully Created", null, string.Empty);
        //        return Ok(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Something went wrong", null, ex.Message);
        //    }
        //}

        //[HttpPut]
        //public async Task<ActionResult<APIResponse>> UpdateCapacityById([FromBody] Capacity dto)
        //{
        //    try
        //    {
        //        var existingResult = await _CapacityRepo.GetAsync(x => x.Id == dto.Id, null, true);
        //        if (existingResult == null)
        //            return _apiResponse.ResponseToClient(false, HttpStatusCode.NotFound, "Could not find capacity", null, string.Empty);

        //        existingResult = _mapper.Map<Capacity>(dto);
        //        dto.Id = existingResult.Id;
        //        await _CapacityRepo.UpdateAsync(dto);

        //        var response = _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successfully retrieved data", null, string.Empty);
        //        return Ok(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Something went wrong", null, ex.Message);
        //    }
        //}

        //[HttpDelete("{Id}")]
        //public async Task<ActionResult<APIResponse>> RemoveCapacity(int Id)
        //{
        //    try
        //    {
        //        if(Id <= 0)
        //            return _apiResponse.ResponseToClient(false, HttpStatusCode.BadRequest, "No data sent", null, string.Empty);

        //        var existingResult = await _CapacityRepo.GetAsync(x => x.Id == Id);
        //        if (existingResult == null)
        //            return _apiResponse.ResponseToClient(false, HttpStatusCode.NotFound, "Could not be found", null, string.Empty);

        //        await _CapacityRepo.DeleteAsync(existingResult);
        //        var response = _apiResponse.ResponseToClient(false, HttpStatusCode.OK, "No data sent", true, string.Empty);
        //        return Ok(response);
        //    }
        //    catch(Exception ex)
        //    {
        //        return _apiResponse.ResponseToClient(false, HttpStatusCode.InternalServerError, "Something went wrong", null, ex.Message);
        //    }
        //}

    //}
}
