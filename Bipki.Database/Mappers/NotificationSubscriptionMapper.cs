using Bipki.Database.Models;
using DbSubscription = Bipki.Database.Models.BusinessModels.NotificationSubscription;

namespace Bipki.Database.Mappers;

public static class NotificationSubscriptionMapper
{
    public static NotificationSubscription? Map(DbSubscription? sub)
        => sub is null
            ? null
            : new NotificationSubscription
            {
                Id = sub.Id,
                SubscribentId = sub.SubscribentId,
                PushEndpoint = sub.PushEndpoint,
                PushAuth = sub.PushAuth,
                PushP256DH = sub.PushP256DH
            };
    
    public static DbSubscription? Map(NotificationSubscription? sub)
        => sub is null
            ? null
            : new DbSubscription
            {
                Id = sub.Id,
                SubscribentId = sub.SubscribentId,
                PushEndpoint = sub.PushEndpoint,
                PushAuth = sub.PushAuth,
                PushP256DH = sub.PushP256DH
            };
}