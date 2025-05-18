using Bipki.Database.Models;

namespace Bipki.Database.Repositories;

public interface INotificationSubscriptionRepository
{
    Guid? Create(NotificationSubscription sub);

    IEnumerable<NotificationSubscription> GetAll(Guid topicId)
    {
        
    }
}