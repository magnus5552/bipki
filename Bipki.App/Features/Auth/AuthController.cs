using System.Security.Claims;
using Bipki.App.Options;
using Bipki.Database.Models.UserModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using User = Bipki.Database.Models.User;

namespace Bipki.App.Features.Auth;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly AuthorizationOptions authOptions;
    private readonly UserManager<User> userManager;

    public AuthController(
        IOptions<AuthorizationOptions> authOptions,
        UserManager<User> userManager)
    {
        this.authOptions = authOptions.Value;
        this.userManager = userManager;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserAuthRequest request)
    {
        var existingUser = await userManager.FindByNameAsync(request.Telegram);
        if (existingUser != null)
        {
            return BadRequest("User already exists");
        }
        
        var user = new User
        {
            UserName = request.Telegram,
            Name = request.Name,
            Surname = request.Surname,
            Telegram = request.Telegram
        };

        var result = await userManager.CreateAsync(user);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Name),
            new(ClaimTypes.Surname, user.Surname),
            new("Telegram", user.Telegram),
            new(ClaimTypes.Role, Roles.User)
        };

        await userManager.AddClaimsAsync(user, claims);

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddDays(365)
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);

        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserAuthRequest request)
    {
        var user = await userManager.FindByNameAsync(request.Telegram);
        if (user == null || 
            user.Name != request.Name || 
            user.Surname != request.Surname)
        {
            return Unauthorized("Invalid credentials");
        }

        var claims = await userManager.GetClaimsAsync(user);
        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddDays(365)
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);

        return Ok(new LoginResponse
        {
            User = user,
            Role = Roles.User
        });
    }

    [HttpPost("login/admin")]
    public async Task<IActionResult> LoginAdmin([FromBody] Guid token)
    {
        if (token != authOptions.AdminToken)
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
            ExpiresUtc = DateTimeOffset.UtcNow.AddDays(365)
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);

        var user = new User
        {
            Name = "Admin",
            Surname = "User",
            Telegram = "admin"
        };

        return Ok(new LoginResponse
        {
            User = user,
            Role = Roles.Admin
        });
    }
}