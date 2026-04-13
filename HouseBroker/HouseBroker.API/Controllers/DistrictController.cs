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
public class DistrictController(IDistrictService _districtService) : ControllerBase
{
    private long UserId => User.FindFirstValue("Id") != null ? long.Parse(User.FindFirstValue("Id")??string.Empty) : 0;

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var districts = await _districtService.GetAllAsync();
        return Ok(new APIResponse(districts));
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(long id)
    {
        var district = await _districtService.GetByIdAsync(id);
        if (district == null) return NotFound(new APIResponse("District not found", null, HttpStatusCode.NotFound));
        return Ok(new APIResponse(district));
    }

    [HttpGet("by-province/{provinceId}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetByProvinceId(long provinceId)
    {
        var districts = await _districtService.GetByProvinceIdAsync(provinceId);
        return Ok(new APIResponse(districts));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UpsertDistrictDto districtDto)
    {
        await _districtService.CreateAsync(districtDto, UserId);
        return Ok(new APIResponse("District created successfully"));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, [FromBody] UpsertDistrictDto districtDto)
    {
        await _districtService.UpdateAsync(id, districtDto, UserId);
        return Ok(new APIResponse("District updated successfully"));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        await _districtService.DeleteAsync(id);
        return Ok(new APIResponse("District deleted successfully"));
    }
}
