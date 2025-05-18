using Bipki.Database.Models.UserModels;

namespace Bipki.Database.Repositories;

public interface IUserRepository
{
    User? GetUser(Guid id);
    User? GetUserByCredentials(string telegram, Guid? conferenceId);
    bool TrySetCheckedIn(Guid userId);
    User? GetByTelegram(string telegram);
}