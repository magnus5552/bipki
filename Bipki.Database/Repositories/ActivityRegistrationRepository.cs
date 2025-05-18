using Bipki.Database.Mappers;
using Bipki.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Bipki.Database.Repositories;

public class ActivityRegistrationRepository : IActivityRegistrationRepository
{
    private readonly BipkiContext dbContext;

    public ActivityRegistrationRepository(BipkiContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<ActivityRegistration?> GetVerifiedByActivityAndUserId(Guid activityId, Guid userId)
    {
        var entity = await dbContext.ActivityRegistrations
            .Where(x =>
                x.Verified &&
                x.ActivityId == activityId &&
                x.UserId == userId).FirstOrDefaultAsync();

        return ActivityRegistrationMapper.Map(entity);
    }

    public async Task<IEnumerable<ActivityRegistration>> GetAllUnverified()
    {
        var unverifiedRegistrations =
            dbContext.ActivityRegistrations.Where(r => !r.Verified);
        return unverifiedRegistrations.Select(r => ActivityRegistrationMapper.Map(r)!).ToList();
    }
    
    public async Task<IEnumerable<ActivityRegistration>> GetAllInActivity(Guid activityId)
    {
        var activityRegistrations = dbContext.ActivityRegistrations.Where(r => r.ActivityId == activityId);
        return activityRegistrations.Select(r => ActivityRegistrationMapper.Map(r)!).ToList();
    }

    public async Task<IEnumerable<ActivityRegistration>> GetAllInUser(Guid userId)
    {
        var activityRegistration = dbContext.ActivityRegistrations.Where(r => r.UserId == userId);
        return activityRegistration.Select(r => ActivityRegistrationMapper.Map(r)!).ToList();
    }
}