using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HouseBroker.Test.ApiTests.APITestHelper;

public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public static long UserId = 1;
    public static string UserRole = "Admin";

    public TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger, UrlEncoder encoder)
        : base(options, logger, encoder)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey("Authorization") || 
            Request.Headers["Authorization"] != "TestScheme")
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        var claims = new[] { 
            new Claim("Id", UserId.ToString()),
            new Claim(ClaimTypes.Role, UserRole),
            new Claim(ClaimTypes.Name, "TestUser")
        };
        var identity = new ClaimsIdentity(claims, "TestScheme");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, "TestScheme");

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}