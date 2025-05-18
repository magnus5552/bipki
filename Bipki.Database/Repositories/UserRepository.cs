using Bipki.Database.Models.UserModels;

namespace Bipki.Database.Repositories;

public class UserRepository : IUserRepository
{
    private readonly BipkiContext dbContext;

    public UserRepository(BipkiContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public User? GetUser(Guid id)
    {
        return dbContext.Users.FirstOrDefault(x => x.Id == id);
    }

    public User? GetByTelegram(string telegram)
    {
        return dbContext.Users.FirstOrDefault(x => x.Telegram == telegram);
    }

    public User? GetUserByCredentials(string telegram, Guid? conferenceId)
    {
        var user = dbContext.Users.FirstOrDefault(x =>
            x.Telegram == telegram &&
            x.ConferenceId == conferenceId);

        return user;
    }

    public bool TrySetCheckedIn(Guid userId)
    {
        var user = dbContext.Users.FirstOrDefault(u => u.Id == userId);
        if (user is null)
            return false;
        if (user.CheckedIn)
            return false;
        user.CheckedIn = true;
        dbContext.SaveChanges();

        return true;
    }
}