using Bipki.Database.Models.BusinessModels;

namespace Bipki.Database.Repositories;

public interface IChatRepository
{
    Task<Chat?> GetById(Guid id);
}