using Bipki.Database.Models;
using DatabaseConference = Bipki.Database.Models.BusinessModels.Conference;

namespace Bipki.Database.Mappers;

public static class ConferenceMapper
{
    public static Conference? Map(DatabaseConference? conference)
        => conference is null
            ? null
            : new Conference
            {
                Id = conference.Id,
                Name = conference.Name,
                Description = conference.Description,
                Plan = conference.Plan,
                StartDate = conference.StartDate,
                EndDate = conference.EndDate,
                Location = conference.Location,
                ChatId = conference.ChatId
            };

    public static DatabaseConference? Map(Conference? conference) =>
        conference is null
            ? null
            : new DatabaseConference
            {
                Id = conference.Id,
                Name = conference.Name,
                Description = conference.Description,
                Plan = conference.Plan,
                Location = conference.Location,
                StartDate = conference.StartDate,
                EndDate = conference.EndDate,
                ChatId = conference.ChatId
            };
}