using Bipki.Database;
using ActivityDto = Bipki.Database.Models.Activity;

namespace Bipki.App.Features.Activity;

public class RegistrationsManager
{
    private readonly BipkiContext dbContext;

    public RegistrationsManager(BipkiContext dbContext) => this.dbContext = dbContext;
    
    public  Guid? RegisterFor(ActivityDto activity, Guid userId)
    {
        var dbActivity = dbContext.Activities.FirstOrDefault(a => a.Id == activity.Id);
        if (dbActivity is null)
            return null;
    }

    public Guid? Unregister(ActivityDto activity, Guid userId)
    {
        
    }

    public Guid EnterWaitList(ActivityDto activity, Guid userId)
    {
        
    }
}

public static class RegistrationManagerExtensions
{
    public static RegistrationResult RegisterOrWaitlist(ActivityDto activity, Guid userId);
}

public enum RegistrationResult
{
    Unknown,
    Registered,
    WaitListed,
    Unregistered
}