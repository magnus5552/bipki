using Bipki.Database.Mappers;
using Bipki.Database.Models;
using Microsoft.EntityFrameworkCore;
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

    public UserActivity? GetUserActivity(Guid userId, Guid activityId)
    {
        var activity = dbContext.Activities.FirstOrDefault(a => a.Id == activityId);
        var user = dbContext.Users.FirstOrDefault(u => u.Id == userId);
        if (activity is null | user is null)
            return null;

        var registration =
            dbContext.ActivityRegistrations.FirstOrDefault(r => r.UserId == userId && r.ActivityId == activityId);
        var status = RegistrationStatus.NotRegistered;
        if (registration is not null)
        {
            status = registration.Verified ? RegistrationStatus.Registered : RegistrationStatus.PendingConfirmation;
        }
        else
        {
            var waitListEntry =
                dbContext.WaitListEntries.FirstOrDefault(w => w.UserId == userId && w.ActivityId == activityId);
            if (waitListEntry is not null)
                status = RegistrationStatus.WaitingList;
        }

        var occupiedSeats = dbContext.ActivityRegistrations.Count(r => r.ActivityId == activityId);
        return new UserActivity
        {
            Id = activityId,
            Title = activity.Name,
            StartDateTime = activity.StartsAt,
            EndDateTime = activity.EndsAt,
            NotificationEnabled = registration?.NotificationEnabled ?? false,
            CommunicationChannel = "stub", // TODO chat integration
            Type = activity.Type,
            RegistrationStatus = status,
            ConfirmationDeadline = status == RegistrationStatus.PendingConfirmation
                ? registration!.RegisteredAt + TimeSpan.FromMinutes(15)
                : null,
            TotalSeats = activity.TotalParticipants,
            OccupiedSeats = occupiedSeats
        };
    }

    public async Task SaveAsync(Activity activity)
    {
        var dbActivity = ActivityMapper.Map(activity);
        if (dbActivity is null)
        {
            return;
        }

        await dbContext.Activities.AddAsync(dbActivity);
        await dbContext.SaveChangesAsync();
    }

    public async Task ChangeAsync(Activity activity)
    {
        var dbActivity = ActivityMapper.Map(activity);

        if (dbActivity is null)
        {
            return;
        }
        
        var existingActivity = await dbContext.Activities.FindAsync(dbActivity.Id);
        if (existingActivity != null)
        {
            dbContext.Entry(existingActivity).CurrentValues.SetValues(dbActivity);
        }
        else
        {
            dbContext.Activities.Update(dbActivity);
        }
    }
    
    public async Task<Activity?> GetByChatId(Guid chatId)
    {
        var activity = await dbContext.Activities
            .Where(x => x.ChatId == chatId)
            .FirstOrDefaultAsync();

        return ActivityMapper.Map(activity);
    }
}