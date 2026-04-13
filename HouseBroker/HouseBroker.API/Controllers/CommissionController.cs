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
public class CommissionController(ICommissionService _commissionService) : ControllerBase
{
    private long UserId => User.FindFirstValue("Id") != null ? long.Parse(User.FindFirstValue("Id")??string.Empty) : 0;

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var commissions = await _commissionService.GetAllAsync();
        return Ok(new APIResponse(commissions));
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(long id)
    {
        var commission = await _commissionService.GetByIdAsync(id);
        if (commission == null) return NotFound(new APIResponse("Commission setting not found", null, HttpStatusCode.NotFound));
        return Ok(new APIResponse(commission));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UpsertCommissionDto dto)
    {
        await _commissionService.CreateAsync(dto, UserId);
        return Ok(new APIResponse("Commission setting created successfully"));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, [FromBody] UpsertCommissionDto dto)
    {
        await _commissionService.UpdateAsync(id, dto, UserId);
        return Ok(new APIResponse("Commission setting updated successfully"));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        await _commissionService.DeleteAsync(id);
        return Ok(new APIResponse("Commission setting deleted successfully"));
    }
}
