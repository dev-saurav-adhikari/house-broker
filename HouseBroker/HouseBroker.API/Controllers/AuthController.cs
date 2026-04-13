using HouseBroker.Application.Common;
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

        [HttpPost("register-house-seeker")]
        public async Task<IActionResult> RegisterHouseSeeker(RegisterRequestDto registerRequest)
        {
            await _authService.RegisterHouseSeeker(registerRequest);
            return Ok(new APIResponse("User has been registered"));
        }
        [HttpPost("register-broker")]
        public async Task<IActionResult> RegisterBroker(RegisterRequestDto registerRequest)
        {
            await _authService.RegisterHouseSeeker(registerRequest);
            return Ok(new APIResponse("User has been registered"));
        }
    }
}
