using Bipki.Database.Models.BusinessModels;

namespace Bipki.Database.Models;

public class UserActivity
{
    public Guid Id { get; set; }
    
    public string Title { get; set; }
    
    public DateTime StartDateTime { get; set; }
    
    public DateTime EndDateTime { get; set; }
    
    public bool NotificationEnabled { get; set; }
    
    public string CommunicationChannel { get; set; }
    
    public ActivityType Type { get; set; }
    
    public RegistrationStatus RegistrationStatus { get; set; }
    
    public int TotalSeats { get; set; }
    
    public int OccupiedSeats { get; set; }
    
    public DateTime? ConfirmationDeadline { get; set; }
}

public enum RegistrationStatus
{
    NotRegistered,
    Registered,
    WaitingList,
    PendingConfirmation
}