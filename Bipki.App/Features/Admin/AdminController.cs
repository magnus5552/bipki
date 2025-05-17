using Bipki.App.Options;
using Bipki.Database.Models;
using Bipki.Database.Models.UserModels;
using Bipki.Database.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Bipki.App.Features.Admin;

[ApiController]
[Route("admin")]
public class AdminController : ControllerBase
{
    private readonly IConferenceRepository conferenceRepository;
    private readonly IUserRepository userRepository;

    public AdminController(IConferenceRepository conferenceRepository, IUserRepository userRepository)
    {
        this.conferenceRepository = conferenceRepository;
        this.userRepository = userRepository;
    }

    [Authorize(Roles = Roles.User)]
    [HttpGet("huy")]
    public async Task<IActionResult> GetHuy()
    {
        return Ok("huy");
    }

    [Authorize(Roles = Roles.Admin)]
    [HttpGet("huy2")]
    public async Task<IActionResult> GetHuy2()
    {
        return Ok("huy2");
    }
    

    [Authorize(Roles = Roles.Admin)]
    [HttpPatch("checkInGuest/{userId:guid}")]
    public async Task<IActionResult> CheckInGuest([FromRoute] Guid userId)
    {
        return userRepository.TrySetCheckedIn(userId) ? NoContent() : BadRequest();
    }
}