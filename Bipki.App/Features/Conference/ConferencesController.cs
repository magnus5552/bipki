using Bipki.App.Features.Conference.Requests;
using Bipki.Database.Models;
using Bipki.Database.Models.UserModels;
using Bipki.Database.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QRCoder;

namespace Bipki.App.Features.Conference;

[Route("conferences")]
public class ConferencesController : ControllerBase
{
    private readonly IConferenceRepository conferenceRepository;
    private readonly UserManager<User> userManager;
    private readonly IChatRepository chatRepository;

    public ConferencesController(IConferenceRepository conferenceRepository, UserManager<User> userManager,
        IChatRepository chatRepository)
    {
        this.conferenceRepository = conferenceRepository;
        this.userManager = userManager;
        this.chatRepository = chatRepository;
    }

    [HttpGet("{conferenceId:guid}")]
    public async Task<IActionResult> GetConference([FromRoute] Guid conferenceId)
    {
        var conference = await conferenceRepository.GetById(conferenceId);
        if (conference is null)
        {
            return NotFound($"Conference with id: {conferenceId} does not exist");
        }

        return Ok(conference);
    }

    [Authorize(Roles = Roles.Admin)]
    [HttpPatch("{conferenceId}")]
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
        conference.Plan = patchRequest.Plan ?? conference.Plan;
        conference.Location = patchRequest.Location ?? conference.Location;
        conference.StartDate = patchRequest.StartDate ?? conference.StartDate;
        conference.EndDate = patchRequest.EndDate ?? conference.EndDate;

        await conferenceRepository.ChangeConference(conference);

        return NoContent();
    }

    [Authorize(Roles = Roles.Admin)]
    [HttpPut]
    public async Task<IActionResult> CreateConference([FromBody] CreateConferenceRequest createRequest)
    {
        var conference = new Database.Models.Conference
        {
            Id = Guid.NewGuid(),
            Plan = createRequest.Plan,
            Name = createRequest.Name,
            Description = createRequest.Description,
            Location = createRequest.Location,
            StartDate = createRequest.StartDate,
            EndDate = createRequest.EndDate,
            ChatId = Guid.NewGuid()
        };
        
        await conferenceRepository.AddConference(conference);
        await chatRepository.Add(new Chat
        {
            Title = conference.Name,
            Type = ChatType.Conference,
            Id = conference.ChatId
        });
        
        
        return Created($"admin/conferences/{conference.Id}", conference.Id);
    }

    [Authorize]
    [HttpGet("{conferenceId:guid}/qrcode")]
    public IActionResult GetConferencEntranceQr([FromRoute] Guid conferenceId)
    {
        if (!Guid.TryParse(userManager.GetUserId(User), out var userId))
            return Unauthorized();

        // TODO you see the problem, I believe
        var qrCodeData =
            QRCodeGenerator.GenerateQrCode(
                new PayloadGenerator.Url($"http://localhost/api/admin/checkInGuest/{userId}"));

        return Ok(new PngByteQRCode(qrCodeData).GetGraphic(10));
    }
}