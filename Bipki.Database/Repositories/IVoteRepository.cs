using Bipki.Database.Models;

namespace Bipki.Database.Repositories;

public interface IVoteRepository
{
    Task AddAsync(Vote vote);

    IEnumerable<Vote> GetByPollOptionId(Guid pollOptionId);
}