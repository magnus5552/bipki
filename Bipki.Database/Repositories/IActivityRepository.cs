using Bipki.Database.Models;

namespace Bipki.Database.Repositories;

public interface IActivityRepository
{
    Activity? GetById(Guid id);
    UserActivity? GetUserActivity(Guid userId, Guid activityId);
    Task SaveAsync(Activity activity);

    Task<Activity?> GetByChatId(Guid id);

    Task ChangeAsync(Activity activity);
}