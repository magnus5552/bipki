using Bipki.Database.Models;

namespace Bipki.Database.Repositories;

public interface IConferenceRepository
{
    Task<Conference?> GetById(Guid id);

    IEnumerable<Conference> GetAllConferences();

    Task AddConference(Conference conference);

    Task ChangeConference(Conference conference);

    Task DeleteAsync(Guid id);
}