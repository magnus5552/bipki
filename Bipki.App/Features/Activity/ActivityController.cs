using Bipki.App.Features.Activity.Create.Dto;
using Bipki.App.Features.Activity.Update.Dto;
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
    private readonly IChatRepository chatRepository;

    public ActivitiesController(IActivityRepository activityRepository, UserManager<User> userManager,
        RegistrationsManager registrationsManager,
        IChatRepository chatRepository)
    {
        this.activityRepository = activityRepository;
        this.userManager = userManager;
        this.registrationsManager = registrationsManager;
        this.chatRepository = chatRepository;
    }
    
    [HttpGet("{activityId:guid}")]
    public IActionResult GetActivity([FromRoute] Guid activityId)
    {
        return Ok(activityRepository.GetById(activityId));
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
        
        activity.Name = request.Name ?? activity.Name;
        activity.Description = request.Description ?? activity.Description;
        activity.TypeLabel = request.TypeLabl ?? activity.TypeLabel;
        activity.StartsAt = request.StartTime ?? activity.StartsAt;
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

        var registrationId = await registrationsManager.Register(activity, userId);
        if (registrationId is null)
            return Conflict();
        return Ok(registrationId);
    }

    [HttpGet("{activityId:guid}/unregister")]
    [Authorize]
    public async Task<IActionResult> Unregister([FromRoute] Guid activityId, [FromRoute] Guid conferenceId)
    {
        var activity = activityRepository.GetById(activityId);
        if (activity is null || activity.ConferenceId != conferenceId)
            return BadRequest();
        
        var userId = Guid.Parse(userManager.GetUserId(User)!); // nullable suppression can never go wrong

        if (await registrationsManager.Unregister(activity, userId))
            return Ok();
        return Conflict();
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