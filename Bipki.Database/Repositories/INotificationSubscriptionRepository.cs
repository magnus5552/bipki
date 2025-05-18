using Bipki.Database.Models;

namespace Bipki.Database.Repositories;

public interface INotificationSubscriptionRepository
{
    Task<Guid?> CreateAsync(NotificationSubscription sub);
    IEnumerable<NotificationSubscription> GetRelevant(Guid userId);
}