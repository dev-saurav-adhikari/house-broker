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
 * ProvinceController is responsible for managing province-related geographical data
 */
public class ProvinceController(IProvinceService _provinceService) : ControllerBase
{
    // this is for to extract the user id from the access token
    private long UserId => User.FindFirstValue("Id") != null ? long.Parse(User.FindFirstValue("Id")??string.Empty) : 0;

    /// <summary>
    /// Get all provinces
    /// </summary>
    /// <returns> List of provinces wrapped by APIResponse</returns>
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var provinces = await _provinceService.GetAllAsync();
        return Ok(new APIResponse(provinces));
    }

    /// <summary>
    /// Get province by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns> Province details wrapped by APIResponse</returns>
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(long id)
    {
        var province = await _provinceService.GetByIdAsync(id);
        if (province == null) return NotFound(new APIResponse("Province not found", null, HttpStatusCode.NotFound));
        return Ok(new APIResponse(province));
    }

    /// <summary>
    /// Create a new province
    /// </summary>
    /// <param name="provinceDto"></param>
    /// <returns> Success message wrapped by APIResponse</returns>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UpsertProvinceDto provinceDto)
    {
        await _provinceService.CreateAsync(provinceDto, UserId);
        return Ok(new APIResponse("Province created successfully"));
    }

    /// <summary>
    /// Update a province
    /// </summary>
    /// <param name="id"></param>
    /// <param name="provinceDto"></param>
    /// <returns> Success message wrapped by APIResponse</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, [FromBody] UpsertProvinceDto provinceDto)
    {
        await _provinceService.UpdateAsync(id, provinceDto, UserId);
        return Ok(new APIResponse("Province updated successfully"));
    }

    /// <summary>
    /// Delete a province
    /// </summary>
    /// <param name="id"></param>
    /// <returns> Success message wrapped by APIResponse</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        await _provinceService.DeleteAsync(id);
        return Ok(new APIResponse("Province deleted successfully"));
    }
}
