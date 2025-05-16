using Bipki.Database.Models;

namespace Bipki.Database.Repositories;

public interface IConferenceRepository
{
    Conference? GetById(Guid id);
}