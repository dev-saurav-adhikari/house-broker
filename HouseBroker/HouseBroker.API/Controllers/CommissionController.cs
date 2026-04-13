using System.Net;
using System.Security.Claims;
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
 * CommissionController is responsible for managing commission settings and calculations
 */
public class CommissionController(ICommissionService _commissionService) : ControllerBase
{
    private long UserId => User.FindFirstValue("Id") != null ? long.Parse(User.FindFirstValue("Id")??string.Empty) : 0;

    /// <summary>
    /// Get all commission settings
    /// </summary>
    /// <returns> List of commission settings wrapped by APIResponse</returns>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var commissions = await _commissionService.GetAllAsync();
        return Ok(new APIResponse(commissions));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        var commission = await _commissionService.GetByIdAsync(id);
        if (commission == null) return NotFound(new APIResponse("Commission setting not found", null, HttpStatusCode.NotFound));
        return Ok(new APIResponse(commission));
    }

    /// <summary>
    /// Create a new commission setting
    /// </summary>
    /// <param name="dto"></param>
    /// <returns> Success message wrapped by APIResponse</returns>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UpsertCommissionDto dto)
    {
        await _commissionService.CreateAsync(dto, UserId);
        return Ok(new APIResponse("Commission setting created successfully"));
    }

    /// <summary>
    /// Update a commission setting
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    /// <returns> Success message wrapped by APIResponse</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, [FromBody] UpsertCommissionDto dto)
    {
        await _commissionService.UpdateAsync(id, dto, UserId);
        return Ok(new APIResponse("Commission setting updated successfully"));
    }

    /// <summary>
    /// Delete a commission setting
    /// </summary>
    /// <param name="id"></param>
    /// <returns> Success message wrapped by APIResponse</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        await _commissionService.DeleteAsync(id);
        return Ok(new APIResponse("Commission setting deleted successfully"));
    }
}
