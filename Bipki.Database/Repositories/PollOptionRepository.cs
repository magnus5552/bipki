using Bipki.Database.Mappers;
using Bipki.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Bipki.Database.Repositories;

public class PollOptionRepository : IPollOptionRepository
{
    private readonly BipkiContext dbContext;

    public PollOptionRepository(BipkiContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task AddPollOptionRangeAsync(IEnumerable<PollOption> entities)
    {
        var dbEntities = entities
            .Select(PollOptionMapper.Map)
            .Where(x => x is not null);

        await dbContext.PollOptions.AddRangeAsync(dbEntities!);
        await dbContext.SaveChangesAsync();
    }

    public async Task<PollOption?> GetById(Guid id)
    {
        var entity = await dbContext.PollOptions.FirstOrDefaultAsync(x => x.Id == id);
        return PollOptionMapper.Map(entity);
    }

    public IEnumerable<PollOption> GetByPollId(Guid id)
    {
        var entities = dbContext.PollOptions.Where(x => x.PollId == id);
        return entities
            .AsEnumerable()
            .Select(PollOptionMapper.Map)
            .Where(x => x is not null)!;
    }
}