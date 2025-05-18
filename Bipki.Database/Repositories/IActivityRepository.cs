using Bipki.Database.Models;

namespace Bipki.Database.Repositories;

public interface IActivityRepository
{
    Activity? GetById(Guid id);
    UserActivity? GetUserActivity(Guid userId, Guid activityId);
    Guid? Save(Activity activity);
}