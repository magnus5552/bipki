using Bipki.Database.Mappers;
using Bipki.Database.Models;

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
        return UserMapper.Map(dbContext.Users.FirstOrDefault(x => x.Id == id));
    }
}