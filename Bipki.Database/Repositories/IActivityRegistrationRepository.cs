using Bipki.Database.Models;

namespace Bipki.Database.Repositories;

public interface IActivityRegistrationRepository
{
    Task<ActivityRegistration?> GetVerifiedByActivityAndUserId(Guid activityId, Guid userId);
}