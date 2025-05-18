using Bipki.Database.Mappers;
using Microsoft.EntityFrameworkCore;
using Poll = Bipki.Database.Models.Poll;

namespace Bipki.Database.Repositories;

public class PollRepository : IPollRepository
{
    private readonly BipkiContext dbContext;

    public PollRepository(BipkiContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task AddPollAsync(Poll poll)
    {
        var dbPoll = PollMapper.Map(poll);
        if (dbPoll is null)
        {
            return;
        }

        await dbContext.Polls.AddAsync(dbPoll);
        await dbContext.SaveChangesAsync();
    }

    public async Task<Poll?> GetByIdAsync(Guid id)
    {
        var entity = await dbContext.Polls.FirstOrDefaultAsync(x => x.Id == id);
        return PollMapper.Map(entity);
    }
}