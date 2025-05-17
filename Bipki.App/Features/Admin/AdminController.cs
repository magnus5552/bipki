using Bipki.App.Features.Admin.Requests;
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
    [HttpPut("conferences")]
    public async Task<IActionResult> CreateConference([FromBody] CreateConferenceRequest createRequest)
    {
        var conference = new Database.Models.Conference
        {
            Id = Guid.NewGuid(),
            Plan = "",
            Name = createRequest.Name,
            Description = createRequest.Description,
            Location = createRequest.Location,
            StartDate = createRequest.StartDate,
            EndDate = createRequest.EndDate
        };

        await conferenceRepository.AddConference(conference);

        return Created($"admin/conferences/{conference.Id}", conference.Id);
    }

    [Authorize(Roles = Roles.Admin)]
    [HttpPatch("conferences/{conferenceId}")]
    public async Task<IActionResult> PatchConference([FromRoute] Guid conferenceId,
        [FromBody] PatchConferenceRequest patchRequest)
    {
        var conference = await conferenceRepository.GetById(conferenceId);
        if (conference is null)
        {
            return NotFound($"Conference with id:{conferenceId} does not exist");
        }

        conference.Description = patchRequest.Description ?? conference.Description;
        conference.Name = patchRequest.Name ?? conference.Name;
        conference.Location = patchRequest.Location ?? conference.Location;
        conference.StartDate = patchRequest.StartDate ?? conference.StartDate;
        conference.EndDate = patchRequest.EndDate ?? conference.EndDate;

        await conferenceRepository.ChangeConference(conference);

        return NoContent();
    }

    [Authorize(Roles = Roles.Admin)]
    [HttpPatch("checkInGuest/{userId:guid}")]
    public async Task<IActionResult> CheckInGuest([FromRoute] Guid userId)
    {
        return userRepository.TrySetCheckedIn(userId) ? NoContent() : BadRequest();
    }
}