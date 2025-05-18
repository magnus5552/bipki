using Bipki.Database.Models;

namespace Bipki.Database.Repositories;

public interface IChatRepository
{
    Task<Chat?> GetById(Guid id);

    Task Add(Chat chat);
}