using Bipki.Database.Models.UserModels;

namespace Bipki.Database.Repositories;

public interface IUserRepository
{
    User? GetUser(Guid id);
    User? GetUserByCredentials(string name, string surname, string telegram, Guid conferenceId);
    bool TrySetCheckedIn(Guid userId);
}