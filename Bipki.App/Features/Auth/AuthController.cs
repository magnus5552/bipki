using System.Security.Claims;
using Bipki.App.Options;
using Bipki.Database.Models.UserModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Bipki.App.Features.Auth;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly AuthorizationOptions _authOptions;

    public AuthController(IOptions<AuthorizationOptions> authOptions)
    {
        _authOptions = authOptions.Value;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (request.Token != _authOptions.AdminToken)
        {
            return Unauthorized("Invalid token");
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.Role, Roles.Admin)
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);

        return Ok(new { message = "Successfully authenticated" });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Ok(new { message = "Successfully logged out" });
    }
}

public class LoginRequest
{
    public Guid Token { get; set; }
} 