using System.Net;
using System.Security.Claims;
using HouseBroker.Application.Common;
using HouseBroker.Application.DTOs;
using HouseBroker.Application.Interfaces.IServices;
using HouseBroker.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HouseBroker.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class BrokerController(IBrokerService _brokerService) : ControllerBase
{
    private long UserId => User.FindFirstValue("Id") != null ? long.Parse(User.FindFirstValue("Id") ?? string.Empty) : 0;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] BasicFilterDto filter)
    {
        var result = await _brokerService.GetBrokersAsync(filter);
        return Ok(new APIResponse(result));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        var result = await _brokerService.GetBrokerByIdAsync(id);
        if (result == null)
        {
            return NotFound(new APIResponse(null, ["Broker not found"], HttpStatusCode.NotFound));
        }
        return Ok(new APIResponse(result));
    }

    [HttpGet("properties")]
    public async Task<IActionResult> GetProperties([FromQuery] BasicFilterDto filter)
    {
        var result = await _brokerService.GetPropertiesByBrokerIdAsync(UserId, filter);
        return Ok(new APIResponse(result));
    }
}
