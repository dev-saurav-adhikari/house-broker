using System.Reflection.Metadata;
using System.Security.Claims;
using HouseBroker.Application.Common;
using HouseBroker.Application.Constants;
using HouseBroker.Application.DTOs;
using HouseBroker.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HouseBroker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PropertyController(IPropertyService _propertyService) : ControllerBase
    {
        private long UserId => User.FindFirstValue("Id") != null ? long.Parse(User.FindFirstValue("Id")) : 0;
        
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllProperties()
        {
            var properties = await _propertyService.GetAllProperties();
            return Ok(new APIResponse(properties));
        }

        [HttpPost]
        public async Task<IActionResult> AddProperties([FromForm]InsertPropertyDto propertyDto)
        {
            await _propertyService.InsertProperty(propertyDto, UserId);
            return Ok(new APIResponse("Property added successfully"));
        }
    }
}
