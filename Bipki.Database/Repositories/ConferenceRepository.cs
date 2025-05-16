using Bipki.Database.Mappers;
using Bipki.Database.Models;

namespace Bipki.Database.Repositories;

public class ConferenceRepository : IConferenceRepository
{
    private readonly BipkiContext dbContext;

    public ConferenceRepository(BipkiContext dbContext)
    {
        this.dbContext = dbContext;
    }
    
    public Conference? GetById(Guid id)
    {
        return ConferenceMapper.Map(dbContext.Conferences.FirstOrDefault(c => c.Id == id));
    }
}