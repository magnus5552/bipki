using Bipki.Database.Models;

namespace Bipki.Database.Mappers;

public static class ActivityMapper
{
    public static Activity? Map(Models.BusinessModels.Activity? activity)
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
                Recording = activity.Recording
            };
}