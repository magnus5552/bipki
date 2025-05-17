using Bipki.Database.Models.UserModels;

namespace Bipki.Database.Repositories;

public class UserRepository: IUserRepository
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

    public User? GetUserByCredentials(string name, string surname, string telegram, Guid  conferenceId)
    {
        var user = dbContext.Users.FirstOrDefault(x => 
            x.Name == name && 
            x.Surname == surname && 
            x.Telegram == telegram &&
            x.ConferenceId == conferenceId);
        return user;
    }
}