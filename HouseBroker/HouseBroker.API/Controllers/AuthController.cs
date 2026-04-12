using HouseBroker.Application.DTOs;
using HouseBroker.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;

namespace HouseBroker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService _authService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto loginRequest)
        {
            var loginResponse = await _authService.Login(loginRequest);
            return Ok(loginResponse);
        }
    }
}
