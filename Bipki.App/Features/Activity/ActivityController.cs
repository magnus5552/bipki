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
        if (!Guid.TryParse(userManager.GetUserId(User), out var userId))
        {
            return Unauthorized();
        }

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
        
        return Created($"activities/{activity.Id}", activity);
    }

    [HttpPatch("{activityId:guid}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> UpdateActivity([FromBody] UpdateActivityRequest request, [FromRoute] Guid activityId,
        [FromRoute] Guid conferenceId)
    {
        var activity = activityRepository.GetById(activityId);
        if (activity is null || activity.ConferenceId != conferenceId)
            return NotFound();
        
        if (request.Name is not null)
            activity.Name = request.Name;
        if (request.Description is not null)
            activity.Description = request.Description;
        if (request.StartTime is not null)
            activity.StartsAt = request.StartTime.Value;
        if (request.EndTime is not null)
            activity.EndsAt = request.EndTime.Value;
        if (request.Type is not null)
            activity.Type = request.Type.Value; // TODO whatever comes with changing the type
        if (request.Recording is not null)
            activity.Recording = request.Recording;
        if (request.TotalSeats is not null)
            activity.TotalSeats = request.TotalSeats.Value; // TODO manslaughter

        await activityRepository.ChangeAsync(activity);
        return NoContent();
    }

    [HttpGet("{activityId:guid}/register")]
    [Authorize]
    public IActionResult Register([FromRoute] Guid activityId, [FromRoute] Guid conferenceId)
    {
        var activity = activityRepository.GetById(activityId);
        if (activity is null || activity.ConferenceId != conferenceId)
            return BadRequest();

        throw new NotImplementedException();
    }

    [HttpGet("{activityId:guid}/unregister")]
    [Authorize]
    public IActionResult Unregister([FromRoute] Guid activityId, [FromRoute] Guid conferenceId)
    {
        var activity = activityRepository.GetById(activityId);
        if (activity is null || activity.ConferenceId != conferenceId)
            return BadRequest();
    }

    [HttpGet("{activityId:guid}/confirmRegistration")]
    [Authorize]
    public IActionResult ConfirmRegistration([FromRoute] Guid activityId, [FromRoute] Guid conferenceId)
    {
        var activity = activityRepository.GetById(activityId);
        if (activity is null || activity.ConferenceId != conferenceId)
            return BadRequest();
        
        
    }
}