using HouseBroker.Application.Common;
using HouseBroker.Application.DTOs;
using HouseBroker.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;

namespace HouseBroker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
  /**
   * AuthController is responsible for handling authentication functionality like login and registration 
   */
    public class AuthController(IAuthService _authService) : ControllerBase
    {

        /// <summary>
        /// Login the user
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns> Access token and refresh token wrapped by APIResponse</returns>
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto loginRequest)
        {
            var loginResponse = await _authService.Login(loginRequest);
            return Ok(new APIResponse(loginResponse));
        }

        /// <summary>
        /// Register the user as house seeker
        /// </summary>
        /// <param name="registerRequest"></param>
        /// <returns> Success message wrapped by APIResponse</returns>
        [HttpPost("register-house-seeker")]
        public async Task<IActionResult> RegisterHouseSeeker(RegisterRequestDto registerRequest)
        {
            await _authService.RegisterHouseSeeker(registerRequest);
            return Ok(new APIResponse("User has been registered"));
        }
        
        /// <summary>
        /// Register the user as  broker
        /// </summary>
        /// <param name="registerRequest"></param>
        /// <returns> Success message wrapped by APIResponse</returns>
        [HttpPost("register-broker")]
        public async Task<IActionResult> RegisterBroker(RegisterRequestDto registerRequest)
        {
            await _authService.RegisterHouseSeeker(registerRequest);
            return Ok(new APIResponse("User has been registered"));
        }
    }
}
