using Bipki.App.Features.Conference.Requests;
using Bipki.App.Features.Notifications;
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
    private readonly IChatRepository chatRepository;
    private readonly UserManager<User> userManager;
    private readonly NotificationsManager notificationsManager;

    public ConferencesController(IConferenceRepository conferenceRepository, UserManager<User> userManager,
        IChatRepository chatRepository, NotificationsManager notificationsManager)
    {
        this.conferenceRepository = conferenceRepository;
        this.userManager = userManager;
        this.chatRepository = chatRepository;
        this.notificationsManager = notificationsManager;
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
    [HttpGet]
    public async Task<IActionResult> GetConferences()
    {
        return Ok(conferenceRepository.GetAllConferences());
    }

    [Authorize(Roles = Roles.Admin)]
    [HttpDelete("{conferenceId}")]
    public async Task<IActionResult> DeleteConference([FromRoute] Guid conferenceId)
    {
        var conference = await conferenceRepository.GetById(conferenceId);
        if (conference is null)
        {
            return NotFound();
        }

        await conferenceRepository.DeleteAsync(conferenceId);
        return Ok();
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

        var sendNotification = false;
        
        conference.Description = patchRequest.Description ?? conference.Description;
        conference.Name = patchRequest.Name ?? conference.Name;
        conference.Plan = patchRequest.Plan ?? conference.Plan;
        conference.Location = patchRequest.Location ?? conference.Location;
        if (patchRequest.StartDate is not null)
        {
            conference.StartDate = patchRequest.StartDate.Value;
            sendNotification = true;
        }
        conference.EndDate = patchRequest.EndDate ?? conference.EndDate;

        await conferenceRepository.ChangeConference(conference);
        await notificationsManager.SendAllInConference(
            Templates.ConferenceNewDate(conference.Name, conference.StartDate),
            conference.Id);

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
        
        await chatRepository.Add(new Chat
        {
            Title = conference.Name,
            Type = ChatType.Conference,
            Id = conference.ChatId
        });
        await conferenceRepository.AddConference(conference);

        
        
        return Created($"conferences/{conference.Id}", conference.Id);
    }

    [Authorize]
    [HttpGet("{conferenceId:guid}/qrcode")]
    public IActionResult GetConferencEntranceQr([FromRoute] Guid conferenceId)
    {
        if (!Guid.TryParse(userManager.GetUserId(User), out var userId))
            return Unauthorized();

        var hostName = HttpContext.Request.Host.Value;
        var qrCodeData =
            QRCodeGenerator.GenerateQrCode(
                new PayloadGenerator.Url($"http://{hostName}/admin/userStatus?userId={userId}"));

        return Ok(new PngByteQRCode(qrCodeData).GetGraphic(10));
    }
}