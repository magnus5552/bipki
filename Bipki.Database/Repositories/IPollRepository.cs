using Bipki.Database.Models;

namespace Bipki.Database.Repositories;

public interface IPollRepository
{
    Task AddPollAsync(Poll poll);

    Task<Poll?> GetByIdAsync(Guid id);

    IEnumerable<Poll> GetByChatId(Guid id);
}