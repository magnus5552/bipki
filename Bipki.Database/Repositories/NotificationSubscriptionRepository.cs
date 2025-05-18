using Bipki.Database.Models;

namespace Bipki.Database.Repositories;

public class NotificationSubscriptionRepository : INotificationSubscriptionRepository
{
    Guid? Create(NotificationSubscription sub);
    
}