using Bipki.Database.Mappers;
using Bipki.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Bipki.Database.Repositories;

public class ConferenceRepository : IConferenceRepository
{
    private readonly BipkiContext dbContext;

    public ConferenceRepository(BipkiContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<Conference?> GetById(Guid id)
    {
        var conference = await dbContext.Conferences.FirstOrDefaultAsync(c => c.Id == id);
        
        return ConferenceMapper.Map(conference);
    }

    public IEnumerable<Conference> GetAllConferences()
    {
        return dbContext.Conferences.Select(ConferenceMapper.Map).Where(x => x is not null)!;
    }

    public async Task AddConference(Conference conference)
    {
        var entity = ConferenceMapper.Map(conference);

        if (entity != null)
        {
            await dbContext.Conferences.AddAsync(entity);
        }

        await dbContext.SaveChangesAsync();
    }

    public async Task ChangeConference(Conference conference)
    {
        var dbConference = ConferenceMapper.Map(conference);
        
        if(dbConference is not null)
        {
            var existingConference = await dbContext.Conferences.FindAsync(dbConference.Id);
            if (existingConference != null)
            {
                dbContext.Entry(existingConference).CurrentValues.SetValues(dbConference);
            }
            else
            {
                dbContext.Conferences.Update(dbConference);
            }
        }

        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await dbContext.Conferences.FirstOrDefaultAsync(x => x.Id == id);
        if (entity is null)
        {
            return;
        }

        dbContext.Conferences.Remove(entity);
        await dbContext.SaveChangesAsync();
    }
}