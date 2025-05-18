using Bipki.Database.Mappers;
using Bipki.Database.Models;

namespace Bipki.Database.Repositories;

public class VoteRepository : IVoteRepository
{
    private readonly BipkiContext dbContext;

    public VoteRepository(BipkiContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task AddAsync(Vote vote)
    {
        var entity = VoteMapper.Map(vote);
        if (entity is null)
        {
            return;
        }

        await dbContext.Votes.AddAsync(entity);
        await dbContext.SaveChangesAsync();
    }

    public IEnumerable<Vote> GetByPollOptionId(Guid pollOptionId)
    {
        return dbContext.Votes
            .Where(x => x.PollOptionId == pollOptionId)
            .AsEnumerable()
            .Select(VoteMapper.Map)
            .Where(x => x is not null)!;
        
    }
}