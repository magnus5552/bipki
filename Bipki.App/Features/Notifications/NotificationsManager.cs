using Bipki.Database.Repositories;
using WebPush;

namespace Bipki.App.Features.Notifications;

public class NotificationsManager
{
    private readonly VapidDetails vapidDetails;
    private readonly IWebPushClient webPushClient;
    private readonly INotificationSubscriptionRepository subscriptionRepository;
    private readonly IActivityRegistrationRepository registrationRepository;
    private readonly IUserRepository userRepository;

    public NotificationsManager(IWebPushClient client, INotificationSubscriptionRepository subscriptionRepository,
        IActivityRegistrationRepository registrationRepository, IUserRepository userRepository, VapidDetails vapidDetails)
    {
        this.webPushClient = client;
        this.subscriptionRepository = subscriptionRepository;
        this.registrationRepository = registrationRepository;
        this.userRepository = userRepository;
        this.vapidDetails = vapidDetails;
    }

    public async Task Send(Notification notification, Guid userId)
    {
        foreach (var subscription in subscriptionRepository.GetRelevant(userId))
        {
            await webPushClient.SendNotificationAsync(new PushSubscription
            {
                Endpoint = subscription.PushEndpoint,
                P256DH = subscription.PushP256DH,
                Auth = subscription.PushAuth
            }, notification.ToString(), vapidDetails);
        }
    }

    public async Task SendAllInActivity(Notification notification, Guid activityid)
    {
        foreach (var registration in await registrationRepository.GetAllInActivity(activityid)) 
            await Send(notification, registration.UserId);
    }
    
    public async Task SendAllInConference(Notification notification, Guid conferenceId)
    {
        foreach (var user in userRepository.GetAllInConference(conferenceId))
        foreach (var registration in await registrationRepository.GetAllInUser(user.Id)) 
            await Send(notification, registration.UserId);
    }
}