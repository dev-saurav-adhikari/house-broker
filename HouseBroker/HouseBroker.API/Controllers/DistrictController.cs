using System.Security.Claims;
using System.Net;
using HouseBroker.Application.Common;
using HouseBroker.Application.DTOs;
using HouseBroker.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HouseBroker.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
/**
 * DistrictController is responsible for handling district-related data and geographical information
 */
public class DistrictController(IDistrictService _districtService) : ControllerBase
{
    // this is for to extract the user id from the access token
    private long UserId => User.FindFirstValue("Id") != null ? long.Parse(User.FindFirstValue("Id")??string.Empty) : 0;

    /// <summary>
    /// Get all districts
    /// </summary>
    /// <returns> List of districts wrapped by APIResponse</returns>
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var districts = await _districtService.GetAllAsync();
        return Ok(new APIResponse(districts));
    }

    /// <summary>
    /// Get district by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns> District details wrapped by APIResponse</returns>
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(long id)
    {
        var district = await _districtService.GetByIdAsync(id);
        if (district == null) return NotFound(new APIResponse("District not found", null, HttpStatusCode.NotFound));
        return Ok(new APIResponse(district));
    }

    /// <summary>
    /// Get districts by province id
    /// </summary>
    /// <param name="provinceId"></param>
    /// <returns> List of districts wrapped by APIResponse</returns>
    [HttpGet("by-province/{provinceId}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetByProvinceId(long provinceId)
    {
        var districts = await _districtService.GetByProvinceIdAsync(provinceId);
        return Ok(new APIResponse(districts));
    }

    /// <summary>
    /// Create a new district
    /// </summary>
    /// <param name="districtDto"></param>
    /// <returns> Success message wrapped by APIResponse</returns>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UpsertDistrictDto districtDto)
    {
        await _districtService.CreateAsync(districtDto, UserId);
        return Ok(new APIResponse("District created successfully"));
    }

    /// <summary>
    /// Update a district
    /// </summary>
    /// <param name="id"></param>
    /// <param name="districtDto"></param>
    /// <returns> Success message wrapped by APIResponse</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, [FromBody] UpsertDistrictDto districtDto)
    {
        await _districtService.UpdateAsync(id, districtDto, UserId);
        return Ok(new APIResponse("District updated successfully"));
    }

    /// <summary>
    /// Delete a district
    /// </summary>
    /// <param name="id"></param>
    /// <returns> Success message wrapped by APIResponse</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        await _districtService.DeleteAsync(id);
        return Ok(new APIResponse("District deleted successfully"));
    }
}
