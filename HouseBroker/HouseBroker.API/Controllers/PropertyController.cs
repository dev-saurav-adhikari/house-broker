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
        private long UserId => User.FindFirstValue("Id") != null ? long.Parse(User.FindFirstValue("Id")??string.Empty) : 0;
        
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllProperties([FromQuery]PropertyFilterDto filter)
        {
            var properties = await _propertyService.GetAllPropertiesAsync(filter);
            return Ok(new APIResponse(properties));
        }

        [HttpPost]
        public async Task<IActionResult> AddProperties([FromForm]InsertPropertyDetailDto propertyDetailDto)
        {
            await _propertyService.InsertProperty(propertyDetailDto, UserId);
            return Ok(new APIResponse("Property added successfully"));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProperty(long id, [FromForm]UpdatePropertyDto propertyDto)
        {
            await _propertyService.UpdateProperty(id, propertyDto, UserId);
            return Ok(new APIResponse("Property updated successfully"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProperty(long id)
        {
            await _propertyService.DeleteProperty(id, UserId);
            return Ok(new APIResponse("Property deleted successfully"));
        }
    }
}
