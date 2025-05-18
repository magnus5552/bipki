using Bipki.Database;
using Bipki.Database.Models.BusinessModels;
using Microsoft.EntityFrameworkCore;
using ActivityDto = Bipki.Database.Models.Activity;

namespace Bipki.App.Features.Activity;

public class RegistrationsManager
{
    private readonly BipkiContext dbContext;

    public RegistrationsManager(BipkiContext dbContext) => this.dbContext = dbContext;
    
    public async Task<Guid?> Register(ActivityDto activity, Guid userId)
    {
        var dbActivity = dbContext.Activities.FirstOrDefault(a => a.Id == activity.Id)!;

        var transaction = await dbContext.Database.BeginTransactionAsync();

        var currentRegistrations = SelectForUpdateOnActivity(dbActivity.Id);
        
        if (dbActivity.Type == ActivityType.Workshop && currentRegistrations.Count() == dbActivity.TotalParticipants)
        {
            await transaction.RollbackAsync();
            return null;
        }
        
        var newRegistration = new ActivityRegistration
        {
            ActivityId = dbActivity.Id,
            NotificationEnabled = true,
            RegisteredAt = DateTime.Now,
            UserId = userId,
            Verified = true
        };

        await dbContext.ActivityRegistrations.AddAsync(newRegistration);
        await transaction.CommitAsync();
        return newRegistration.Id;
    }

    public async Task Unregister(Guid activityId, Guid userId)
    {
        var registration = dbContext.ActivityRegistrations.FirstOrDefault(r => r.ActivityId == activityId && r.UserId == userId);
        if (registration is null)
            return;

        dbContext.ActivityRegistrations.Remove(registration);
        await dbContext.SaveChangesAsync();

        await TopWaiterToRegister(activityId);
    }
    
    public async Task<bool> VerifyRegistration(Guid activityId , Guid userId)
    {
        var registration = dbContext.ActivityRegistrations.FirstOrDefault(r => r.ActivityId == activityId && r.UserId == userId);
        if (registration is null || registration.Verified)
            return false;

        registration.Verified = true;
        await dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<Guid?> EnterWaitList(ActivityDto activity, Guid userId)
    {
        var dbActivity = dbContext.Activities.FirstOrDefault(a => a.Id == activity.Id)!;
        if (dbContext.WaitListEntries.FirstOrDefault(w =>
                w.UserId == userId && w.ActivityId == activity.Id) is not null)
            return null;

        var transaction = await dbContext.Database.BeginTransactionAsync();

        SelectForUpdateOnActivity(activity.Id);
        
        var newWaitListEntry = new WaitListEntry
        {
            ActivityId = dbActivity.Id,
            UserId = userId,
            WaitsSince = DateTime.Now
        };

        await dbContext.WaitListEntries.AddAsync(newWaitListEntry);
        await transaction.CommitAsync();

        return newWaitListEntry.Id;
    }
    
    public async Task ExitWaitList(Guid activityId, Guid userId)
    {
        var waitListEntry = dbContext.WaitListEntries.FirstOrDefault(w => w.Id == activityId && w.UserId == userId);
        if (waitListEntry is null)
            return;

        dbContext.WaitListEntries.Remove(waitListEntry);
        await dbContext.SaveChangesAsync();
    }

    public async Task TopWaiterToRegister(Guid activityId)
    {
        var entry = dbContext.WaitListEntries.Where(e => e.ActivityId == activityId)
            .OrderBy(e => e.WaitsSince).FirstOrDefault();
        if (entry is null)
            return;

        await RegisterFromWaitList(entry);
    }
    
    private async Task RegisterFromWaitList(WaitListEntry waitListEntry)
    {
        var activity = dbContext.Activities.FirstOrDefault(r => r.Id == waitListEntry.ActivityId);
        if (activity is null)
            return;

        var newRegistration = new ActivityRegistration
        {
            ActivityId = waitListEntry.ActivityId,
            UserId = waitListEntry.UserId,
            Verified = false,
            RegisteredAt = DateTime.Now
        };

        var transaction = await dbContext.Database.BeginTransactionAsync();

        var registrations = SelectForUpdateOnActivity(waitListEntry.ActivityId);

        if (registrations.Count() == activity.TotalParticipants)
        {
            await transaction.RollbackAsync();
            return;
        }

        await dbContext.ActivityRegistrations.AddAsync(newRegistration);
        waitListEntry.Deleted = true;
        await transaction.CommitAsync();
        
        // TODO send confirmation notification
    }

    public async Task DeleteUnverifiedRegistration(Guid registrationId)
    {
        var registration = dbContext.ActivityRegistrations.FirstOrDefault(r => r.Id == registrationId);
        if (registration is null || registration.Verified) return;

        var activityId = registration.ActivityId;
        dbContext.ActivityRegistrations.Remove(registration);
        await dbContext.SaveChangesAsync();

        await TopWaiterToRegister(activityId);
    }
    
    public async Task Shrink(Guid activityId)
    {
        var activity = dbContext.Activities.FirstOrDefault(a => a.Id == activityId);
        if (activity is null)
            return;

        var transaction = await dbContext.Database.BeginTransactionAsync();

        var registrations = SelectForUpdateOnActivity(activityId).OrderBy(r => r.RegisteredAt);
        dbContext.ActivityRegistrations.RemoveRange(registrations.Skip(activity.TotalParticipants));

        await transaction.CommitAsync();
    }

    private IQueryable<ActivityRegistration> SelectForUpdateOnActivity(Guid activityId) => 
        dbContext.Database.SqlQueryRaw<ActivityRegistration>("SELECT * FROM activity_registrations WHERE id == {activityId} FOR UPDATE", activityId);
}

public static class RegistrationManagerExtensions
{
    public static async Task<RegistrationResult> RegisterOrWaitlist(this RegistrationsManager manager, ActivityDto activity, Guid userId)
    {
        var registrationId = await manager.Register(activity, userId);
        if (registrationId is not null)
            return RegistrationResult.Registered;
        var waitListId = await manager.EnterWaitList(activity, userId);
        return waitListId is not null ? RegistrationResult.WaitListed : RegistrationResult.Unknown;
    }
}

public enum RegistrationResult
{
    Unknown,
    Registered,
    WaitListed,
    Unregistered
}