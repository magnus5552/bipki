using Bipki.Database.Models;

namespace Bipki.Database.Mappers;

public static class ConferenceMapper
{
    public static Conference? Map(Models.BusinessModels.Conference? conference)
        => conference is null
            ? null
            : new Conference
            {
                Id = conference.Id,
                
            };
}