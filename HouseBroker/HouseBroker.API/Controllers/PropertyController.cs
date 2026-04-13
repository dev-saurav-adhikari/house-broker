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
    /**
     * PropertyController is responsible for handling property listings, including searching, creating, and updating properties
     */
    public class PropertyController(IPropertyService _propertyService) : ControllerBase
    {
        // this is for to extract the user id from the access token
        private long UserId => User.FindFirstValue("Id") != null ? long.Parse(User.FindFirstValue("Id")??string.Empty) : 0;
        
        /// <summary>
        /// Get all properties
        /// </summary>
        /// <param name="filter"></param>
        /// <returns> List of properties wrapped by APIResponse</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllProperties([FromQuery]PropertyFilterDto filter)
        {
            var properties = await _propertyService.GetAllPropertiesAsync(filter);
            return Ok(new APIResponse(properties));
        }

        /// <summary>
        /// Add a new property
        /// </summary>
        /// <param name="propertyDetailDto"></param>
        /// <returns> Success message wrapped by APIResponse</returns>
        [HttpPost]
        public async Task<IActionResult> AddProperties([FromForm]InsertPropertyDetailDto propertyDetailDto)
        {
            await _propertyService.InsertProperty(propertyDetailDto, UserId);
            return Ok(new APIResponse("Property added successfully"));
        }

        /// <summary>
        /// Update a property
        /// </summary>
        /// <param name="id"></param>
        /// <param name="propertyDto"></param>
        /// <returns> Success message wrapped by APIResponse</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProperty(long id, [FromForm]UpdatePropertyDto propertyDto)
        {
            await _propertyService.UpdateProperty(id, propertyDto, UserId);
            return Ok(new APIResponse("Property updated successfully"));
        }

        /// <summary>
        /// Delete a property
        /// </summary>
        /// <param name="id"></param>
        /// <returns> Success message wrapped by APIResponse</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProperty(long id)
        {
            await _propertyService.DeleteProperty(id, UserId);
            return Ok(new APIResponse("Property deleted successfully"));
        }
    }
}
