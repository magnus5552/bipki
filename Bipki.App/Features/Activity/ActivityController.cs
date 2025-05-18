using Bipki.App.Features.Activity.Create.Dto;
using Bipki.App.Features.Activity.Update.Dto;
using Bipki.App.Features.Notifications;
using Bipki.Database.Models;
using Bipki.Database.Models.UserModels;
using Bipki.Database.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bipki.App.Features.Activity;

[Route("conferences/{conferenceId:guid}/activities")]
public class ActivitiesController : ControllerBase
{
    private readonly IActivityRepository activityRepository;
    private readonly UserManager<User> userManager;
    private readonly RegistrationsManager registrationsManager;
    private readonly NotificationsManager notificationsManager;
    private readonly IChatRepository chatRepository;

    public ActivitiesController(IActivityRepository activityRepository, UserManager<User> userManager,
        RegistrationsManager registrationsManager, NotificationsManager notificationsManager,
        IChatRepository chatRepository)
    {
        this.activityRepository = activityRepository;
        this.userManager = userManager;
        this.registrationsManager = registrationsManager;
        this.notificationsManager = notificationsManager;
        this.chatRepository = chatRepository;
    }
    
    [HttpGet("{activityId:guid}")]
    public IActionResult GetActivity([FromRoute] Guid activityId)
    {
        return Ok(activityRepository.GetById(activityId));
    }

    [HttpGet]
    public IActionResult GetAllInConference([FromRoute] Guid conferenceId)
    {
        return Ok(activityRepository.GetAllInConference(conferenceId));
    }
    
    [HttpGet("extended")]
    [Authorize]
    public IActionResult GetAllForUser([FromRoute] Guid conferenceId)
    {
        var userId = Guid.Parse(userManager.GetUserId(User)!); // nullable suppression can never go wrong

        return Ok(activityRepository.GetAllInConference(conferenceId)
            .Select(a => activityRepository.GetUserActivity(userId, a.Id)).ToArray());
    }

    [HttpGet("{activityId:guid}/extended")]
    [Authorize]
    public IActionResult GetUserActivity([FromRoute] Guid activityId)
    {
        var userId = Guid.Parse(userManager.GetUserId(User)!); // nullable suppression can never go wrong
        return Ok(activityRepository.GetUserActivity(userId, activityId));
    }
    
    [HttpPost]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> CreateActivity([FromBody] CreateActivityRequest request, [FromRoute] Guid conferenceId)
    {
        if (request.StartTime < request.EndTime || request.TotalSeats < 0)
            return BadRequest();
        
        var activity = new Database.Models.Activity
        {
            Name = request.Name,
            TypeLabel = request.TypeLabel,
            ConferenceId = conferenceId,
            Description = request.Description,
            StartsAt = request.StartTime,
            EndsAt = request.EndTime,
            Type = request.Type,
            TotalSeats = request.TotalSeats,
            Id = Guid.NewGuid()
        };
        
        await activityRepository.SaveAsync(activity);
        
        await chatRepository.Add(new Chat
        {
            Title = activity.Name,
            Type = ChatType.Activity,
            Id = activity.ChatId
        });

        await notificationsManager.SendAllInConference(Templates.NewActivity(activity.Name), conferenceId);
        
        return Created($"conferences/{conferenceId}/activities/{activity.Id}", activity);
    }

    [HttpPatch("{activityId:guid}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> UpdateActivity([FromBody] UpdateActivityRequest request, [FromRoute] Guid activityId,
        [FromRoute] Guid conferenceId)
    {
        var activity = activityRepository.GetById(activityId);
        if (activity is null || activity.ConferenceId != conferenceId)
            return NotFound();

        var sendNotification = false;
        activity.Name = request.Name ?? activity.Name;
        activity.Description = request.Description ?? activity.Description;
        activity.TypeLabel = request.TypeLabl ?? activity.TypeLabel;
        if (request.StartTime is not null)
        {
            activity.StartsAt = request.StartTime.Value;
            sendNotification = true;
        }
        activity.EndsAt = request.EndTime ?? activity.EndsAt;
        activity.Type = request.Type ?? activity.Type; // TODO whatever comes with changing the type
        activity.Recording = request.Recording ?? activity.Recording;
        if (request.TotalSeats is not null)
        {
            if (activity.TotalSeats > request.TotalSeats)
                await registrationsManager.Shrink(activity.Id);
            activity.TotalSeats = request.TotalSeats.Value;
        }

        await activityRepository.ChangeAsync(activity);
        if (sendNotification) 
            await notificationsManager.SendAllInConference(Templates.ActivityNewDate(activity.Name, activity.StartsAt), conferenceId);
        return NoContent();
    }

    [HttpGet("{activityId:guid}/register")]
    [Authorize]
    public async Task<IActionResult> Register([FromRoute] Guid activityId, [FromRoute] Guid conferenceId)
    {
        var activity = activityRepository.GetById(activityId);
        if (activity is null || activity.ConferenceId != conferenceId)
            return BadRequest();

        var userId = Guid.Parse(userManager.GetUserId(User)!); // nullable suppression can never go wrong

        var registrationStatus = await registrationsManager.RegisterOrWaitlist(activity, userId);
        if (registrationStatus is RegistrationResult.Unknown)
            return Conflict();
        return Ok(registrationStatus);
    }

    [HttpGet("{activityId:guid}/unregister")]
    [Authorize]
    public async Task<IActionResult> Unregister([FromRoute] Guid activityId, [FromRoute] Guid conferenceId)
    {
        var activity = activityRepository.GetById(activityId);
        if (activity is null || activity.ConferenceId != conferenceId)
            return BadRequest();
        
        var userId = Guid.Parse(userManager.GetUserId(User)!); // nullable suppression can never go wrong

        var topWaiterId = await registrationsManager.Unregister(activityId, userId);
        if (topWaiterId is not null)
            await notificationsManager.Send(Templates.VerifyRegistration(activity.Name, DateTime.Now + TimeSpan.FromMinutes(15)), topWaiterId.Value);
        
        return Ok();
    }

    [HttpGet("{activityId:guid}/confirmRegistration")]
    [Authorize]
    public async Task<IActionResult> ConfirmRegistration([FromRoute] Guid activityId, [FromRoute] Guid conferenceId)
    {
        var activity = activityRepository.GetById(activityId);
        if (activity is null || activity.ConferenceId != conferenceId)
            return BadRequest();
        
        var userId = Guid.Parse(userManager.GetUserId(User)!); // nullable suppression can never go wrong

        if (await registrationsManager.VerifyRegistration(activityId, userId))
            return Ok();
        return Conflict();
    }
}