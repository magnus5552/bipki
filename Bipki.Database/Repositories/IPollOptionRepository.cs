using Bipki.Database.Models;

namespace Bipki.Database.Repositories;

public interface IPollOptionRepository
{
    Task AddPollOptionRangeAsync(IEnumerable<PollOption> entities);

    Task<PollOption?> GetById(Guid id);

    IEnumerable<PollOption> GetByPollId(Guid id);
}