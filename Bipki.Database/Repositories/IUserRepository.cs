using Bipki.Database.Models;

namespace Bipki.Database.Repositories;

public interface IUserRepository
{
    User? GetUser(Guid id);
}