using Bipki.Database.Models.UserModels;
using Bipki.Database.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bipki.App.Features.Admin;

[ApiController]
[Route("admin")]
public class AdminController : ControllerBase
{
    private readonly IUserRepository userRepository;

    public AdminController(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    [Authorize(Roles = Roles.Admin)]
    [HttpPatch("checkInGuest/{userId:guid}")]
    public async Task<IActionResult> CheckInGuest([FromRoute] Guid userId)
    {
        return userRepository.TrySetCheckedIn(userId) ? NoContent() : BadRequest();
    }
}