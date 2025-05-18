using Bipki.Database.Models.UserModels;
using Bipki.Database.Repositories;
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
    private readonly IConferenceRepository conferenceRepository;
    private readonly IUserRepository userRepository;

    public AuthController(
        IOptions<AuthorizationOptions> authOptions,
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IConferenceRepository conferenceRepository,
        IUserRepository userRepository)
    {
        this.authOptions = authOptions.Value;
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.conferenceRepository = conferenceRepository;
        this.userRepository = userRepository;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserAuthRequest request)
    {
        var existingUser = userRepository.GetUserByCredentials(
            request.Name,
            request.Surname,
            request.Telegram,
            request.ConferenceId);
        
        if (existingUser != null)
        {
            return BadRequest("User already exists");
        }

        var user = new User
        {
            UserName = $"{request.Name} {request.Surname} {request.Telegram}",
            Name = request.Name,
            Surname = request.Surname,
            Telegram = request.Telegram,
            ConferenceId = request.ConferenceId
        };

        var result = await userManager.CreateAsync(user);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        await userManager.AddToRoleAsync(user, Roles.User);
        await signInManager.SignInAsync(user, true);

        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserAuthRequest request)
    {
        var user = userRepository.GetUserByCredentials(
            request.Name,
            request.Surname,
            request.Telegram,
            request.ConferenceId);
        
        if (user == null)
        {
            return Unauthorized("Invalid credentials");
        }

        await signInManager.SignInAsync(user, true);

        var model = new UserModel
        {
            Id = user.Id,
            UserName = user.Name +" " + user.Surname,
            Telegram = user.Telegram,
            Role = Roles.User,
            ConferenceId = user.ConferenceId
        };

        return Ok(model);
    }

    [HttpPost("login/admin")]
    public async Task<IActionResult> LoginAdmin([FromBody] Guid token)
    {
        if (token != authOptions.AdminToken)
        {
            return Unauthorized("Invalid token");
        }

        var user = await userManager.FindByIdAsync("7e0ca8d7-841b-4f0d-92e8-64ed6dd9805a");

        if (user is null)
        {
            return BadRequest("Something went wrong");
        }

        await signInManager.SignInAsync(user, true);

        var model = new UserModel
        {
            Id = user.Id,
            UserName = user.Name + "  " + user.Surname,
            Telegram = user.Telegram,
            Role = Roles.Admin,
            ConferenceId = user.ConferenceId
        };
        
        return Ok(model);
    }
    
    [Authorize]
    [HttpGet("Login")]
    public async Task<IActionResult> Login()
    {
        var user = await userManager.GetUserAsync(HttpContext.User);
        if (user == null)
        {
            return NotFound();
        }
        var model = new UserModel
        {
            Id = user.Id,
            UserName = user.Name + "  " + user.Surname,
            Telegram = user.Telegram,
            Role = Roles.Admin,
            ConferenceId = user.ConferenceId
        };
        return Ok(model);
    }
    
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var user = await userManager.GetUserAsync(HttpContext.User);
        if (user == null)
            return NotFound();
        await signInManager.SignOutAsync();
        return Ok();
    }

    [HttpGet("access-denied")]
    public IActionResult AccessDenied()
    {
        return Unauthorized("Access denied");
    }
}