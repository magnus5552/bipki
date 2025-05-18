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
}