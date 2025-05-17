using Bipki.Database.Models.UserModels;
using Bipki.Database.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bipki.App.Features.Activity;

[Authorize]
[Route("activities")]
public class ActivitiesController : ControllerBase
{
    private readonly IActivityRepository activityRepository;
    private readonly UserManager<User> userManager;

    public ActivitiesController(IActivityRepository activityRepository, UserManager<User> userManager)
    {
        this.activityRepository = activityRepository;
        this.userManager = userManager;
    }
    
    // [HttpGet]
    // [Route("{activityId:guid}")]
    // public IActionResult GetActivity([FromRoute] Guid activityId)
    // {
    //     throw new NotImplementedException();
    // }

    [HttpGet]
    [Route("{activityId:guid}")]
    [Authorize]
    public IActionResult GetUserActivity([FromRoute] Guid activityId)
    {
        if (!Guid.TryParse(userManager.GetUserId(User), out var userId))
        {
            return Unauthorized();
        }

        return Ok(activityRepository.GetUserActivity(userId, activityId));
    }
}