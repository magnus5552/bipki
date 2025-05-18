using Bipki.Database.Mappers;
using Bipki.Database.Models;

namespace Bipki.Database.Repositories;

public class NotificationSubscriptionRepository : INotificationSubscriptionRepository
{
    private readonly BipkiContext dbContext;

    public NotificationSubscriptionRepository(BipkiContext dbContext)
    {
        this.dbContext = dbContext;
    }
    
    public async Task<Guid?> CreateAsync(NotificationSubscription sub)
    {
        var newSubscription = NotificationSubscriptionMapper.Map(sub);
        if (newSubscription is null)
            return null;
        newSubscription.Id = Guid.NewGuid();
        await dbContext.AddAsync(newSubscription);
        return newSubscription.Id;
    }

    public IEnumerable<NotificationSubscription> GetRelevant(Guid userId)
    {
        return dbContext.NotificationSubscriptions.Where(s => s.SubscribentId == userId).Select(s => NotificationSubscriptionMapper.Map(s)!).ToList();
    }
}