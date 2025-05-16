using Bipki.Database.Models;
using DbUser = Bipki.Database.Models.UserModels.User;

namespace Bipki.Database.Mappers;

public static class UserMapper
{
    public static User? Map(DbUser? user) =>
        user is null
            ? null
            : new User
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                Telegram = user.Telegram,
                ConferenceId = user.ConferenceId
            };
}