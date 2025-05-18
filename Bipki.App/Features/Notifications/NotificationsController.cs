using Bipki.App.Features.Notifications.Create;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bipki.App.Features.Notifications;

[Route("conference/{conferenceId}/activities/{activityId}/notifications")]
[Authorize]
public class NotificationsController : ControllerBase
{
    [HttpPost]
    public IActionResult Subscribe([FromBody] CreateNotificationsSubscriptionRequest request)
    {
        return Ok();
    }
}