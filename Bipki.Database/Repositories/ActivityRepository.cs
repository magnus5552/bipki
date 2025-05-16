using Bipki.Database.Mappers;
using Bipki.Database.Models.BusinessModels;
using Activity = Bipki.Database.Models.Activity;

namespace Bipki.Database.Repositories;

public class ActivityRepository : IActivityRepository
{
    private readonly BipkiContext dbContext;

    public ActivityRepository(BipkiContext dbContext)
    {
        this.dbContext = dbContext;
    }
    
    public Activity? GetById(Guid id)
    {
        return ActivityMapper.Map(dbContext.Activities.FirstOrDefault(a => a.Id == id));
    }
}