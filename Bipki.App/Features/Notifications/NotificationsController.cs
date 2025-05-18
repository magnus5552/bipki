using Bipki.App.Features.Notifications.Create;
using Bipki.Database.Mappers;
using Bipki.Database.Models;
using Bipki.Database.Models.UserModels;
using Bipki.Database.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebPush;

namespace Bipki.App.Features.Notifications;

[Route("notifications")]
[Authorize]
public class NotificationsController : ControllerBase
{
    private readonly UserManager<User> userManager;
    private readonly INotificationSubscriptionRepository subscriptionRepository;
    public NotificationsController(UserManager<User> userManager, INotificationSubscriptionRepository subscriptionRepository)
    {
        this.subscriptionRepository = subscriptionRepository;
        this.userManager = userManager;
    }

    [HttpPost]
    public async Task<IActionResult> Subscribe([FromBody] CreateNotificationsSubscriptionRequest request)
    {
        var userId = Guid.Parse(userManager.GetUserId(User)!);
        var id = await subscriptionRepository.CreateAsync(new NotificationSubscription
        {
            PushAuth = request.PushAuth,
            PushEndpoint = request.PushEndpoint,
            PushP256DH = request.PushP256DH
        });

        if (id is null)
            return Conflict();
        return Ok(id);
    }
}