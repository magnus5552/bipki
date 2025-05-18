using Bipki.Database.Models;
using Bipki.Database.Models.UserModels;

namespace Bipki.Database.Repositories;

public interface IActivityRepository
{
    Activity? GetById(Guid id);
    IEnumerable<Activity> GetAllInConference(Guid conferenceId);
    UserActivity? GetUserActivity(Guid userId, Guid activityId);
     
    Task SaveAsync(Activity activity);

    Task<Activity?> GetByChatId(Guid id);

    Task ChangeAsync(Activity activity);
    Task<bool> ExistsAsync(Guid id);
}