using Bipki.Database.Models;
using DbActivity = Bipki.Database.Models.BusinessModels.Activity;

namespace Bipki.Database.Mappers;

public static class ActivityMapper
{
    public static DbActivity? Map(Activity? activity)
        => activity is null
            ? null
            : new DbActivity
            {
                Id = activity.Id,
                ConferenceId = activity.ConferenceId,
                Name = activity.Name,
                Description = activity.Description,
                StartsAt = activity.StartsAt,
                EndsAt = activity.EndsAt,
                Type = activity.Type,
                Recording = activity.Recording,
                ChatId = activity.ChatId
            };
    
    public static Activity? Map(DbActivity? activity)
        => activity is null
            ? null
            : new Activity
            {
                Id = activity.Id,
                ConferenceId = activity.ConferenceId,
                Name = activity.Name,
                Description = activity.Description,
                StartsAt = activity.StartsAt,
                EndsAt = activity.EndsAt,
                Type = activity.Type,
                Recording = activity.Recording,
                ChatId = activity.ChatId
            };
}