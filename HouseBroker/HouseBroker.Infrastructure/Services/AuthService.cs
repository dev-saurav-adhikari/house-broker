using HouseBroker.Application.DTOs;
using HouseBroker.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Identity;

namespace HouseBroker.Infrastructure.Services;

public class AuthService(UserManager<IdentityUser<long>> _userManager,SignInManager<IdentityUser<long>> _signInManager): IAuthService
{
    public async Task<LoginResponseDto> Login(LoginRequestDto  loginRequest)
    {
       var user = await _userManager.FindByNameAsync(loginRequest.Username);
       if(user == null) throw new UnauthorizedAccessException("Invalid username or password");
       var signinResult = await _signInManager.CheckPasswordSignInAsync(user, loginRequest.Password, false);
       if (signinResult.Succeeded)
       {

           return new LoginResponseDto
           {
               AccessToken = "",
               RefreshToken = ""
           };
       }
       throw new Exception("Invalid username or password");
    }
}