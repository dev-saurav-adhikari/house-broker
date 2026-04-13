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
public class ProvinceController(IProvinceService _provinceService) : ControllerBase
{
    private long UserId => User.FindFirstValue("Id") != null ? long.Parse(User.FindFirstValue("Id")??string.Empty) : 0;

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var provinces = await _provinceService.GetAllAsync();
        return Ok(new APIResponse(provinces));
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(long id)
    {
        var province = await _provinceService.GetByIdAsync(id);
        if (province == null) return NotFound(new APIResponse("Province not found", null, HttpStatusCode.NotFound));
        return Ok(new APIResponse(province));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UpsertProvinceDto provinceDto)
    {
        await _provinceService.CreateAsync(provinceDto, UserId);
        return Ok(new APIResponse("Province created successfully"));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, [FromBody] UpsertProvinceDto provinceDto)
    {
        await _provinceService.UpdateAsync(id, provinceDto, UserId);
        return Ok(new APIResponse("Province updated successfully"));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        await _provinceService.DeleteAsync(id);
        return Ok(new APIResponse("Province deleted successfully"));
    }
}
