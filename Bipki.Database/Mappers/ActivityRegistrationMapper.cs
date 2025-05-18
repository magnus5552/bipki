using Bipki.Database.Models;
using DbActivityRegistration = Bipki.Database.Models.BusinessModels.ActivityRegistration;

namespace Bipki.Database.Mappers;

public static class ActivityRegistrationMapper
{
    public static ActivityRegistration? Map(DbActivityRegistration? entity) =>
        entity is null
            ? null
            : new ActivityRegistration
            {
                Id = entity.Id,
                ActivityId = entity.ActivityId,
                UserId = entity.UserId,
                RegisteredAt = entity.RegisteredAt,
                Verified = entity.Verified,
                NotificationEnabled = entity.NotificationEnabled
            };
    
    public static DbActivityRegistration? Map(ActivityRegistration? entity) =>
        entity is null
            ? null
            : new DbActivityRegistration
            {
                Id = entity.Id,
                ActivityId = entity.ActivityId,
                UserId = entity.UserId,
                RegisteredAt = entity.RegisteredAt,
                Verified = entity.Verified,
                NotificationEnabled = entity.NotificationEnabled
            };
}