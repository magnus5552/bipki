using System.Security.Claims;
using Bipki.Database.Models.UserModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using AuthorizationOptions = Bipki.App.Options.AuthorizationOptions;

namespace Bipki.App.Features.Auth;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly AuthorizationOptions authOptions;
    private readonly UserManager<User> userManager;
    private readonly SignInManager<User> signInManager;

    public AuthController(
        IOptions<AuthorizationOptions> authOptions,
        UserManager<User> userManager,
        SignInManager<User> signInManager)
    {
        this.authOptions = authOptions.Value;
        this.userManager = userManager;
        this.signInManager = signInManager;
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
            new("Telegram", user.Telegram)
        };

        await userManager.AddToRoleAsync(user, Roles.User);
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
        await userManager.UpdateSecurityStampAsync(user);
        await signInManager.SignInAsync(user, true);

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
    [HttpGet("access-denied")]
    public IActionResult AccessDenied()
    {
        return Unauthorized("Access denied");
    }
}