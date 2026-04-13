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
/**
 * BrokerController is responsible for handling broker-related functionality like retrieving broker details and their properties
 */
public class BrokerController(IBrokerService _brokerService) : ControllerBase
{
    // this is for to extract the user id from the access token
    private long UserId =>
        User.FindFirstValue("Id") != null ? long.Parse(User.FindFirstValue("Id") ?? string.Empty) : 0;

    /// <summary>
    /// Get all brokers with basic filters
    /// </summary>
    /// <param name="filter"></param>
    /// <returns> List of brokers wrapped by APIResponse</returns>
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] BasicFilterDto filter)
    {
        var result = await _brokerService.GetBrokersAsync(filter);
        return Ok(new APIResponse(result));
    }

    /// <summary>
    /// Get broker by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns> Broker details wrapped by APIResponse</returns>
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

    /// <summary>
    /// Get properties listed by broker (borker id will be extracted from the access token)
    /// </summary>
    /// <param name="filter"></param>
    /// <returns> List of properties wrapped by APIResponse</returns>
    [HttpGet("properties")]
    public async Task<IActionResult> GetProperties([FromQuery] BasicFilterDto filter)
    {
        var result = await _brokerService.GetPropertiesByBrokerIdAsync(UserId, filter);
        return Ok(new APIResponse(result));
    }

    /// <summary>
    /// Get total estimated commission for the broker (broker id will be extracted from the access token)
    /// </summary>
    /// <returns> Total estimated commission wrapped by APIResponse</returns>
    [HttpGet("total-estimated-commission")]
    public async Task<IActionResult> GetTotalEstimatedCommission()
    {
        var amount = await _brokerService.TotalEstimatedCommissionAsync(UserId);
        return Ok(new APIResponse(amount));
    }
}