using Bipki.Database.Models;

namespace Bipki.Database.Repositories;

public interface IActivityRegistrationRepository
{
    Task<ActivityRegistration?> GetVerifiedByActivityAndUserId(Guid activityId, Guid userId);
    Task<IEnumerable<ActivityRegistration>> GetAllUnverified();
    Task<IEnumerable<ActivityRegistration>> GetAllInActivity(Guid activityId);
    Task<IEnumerable<ActivityRegistration>> GetAllInUser(Guid activityId);
}