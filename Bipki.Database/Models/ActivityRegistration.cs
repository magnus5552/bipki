namespace Bipki.Database.Models;

public class ActivityRegistration
{
    public Guid Id { get; set; }
    
    public Guid ActivityId { get; set; }
    
    public Guid UserId { get; set; }
    
    public DateTime RegisteredAt { get; set; }
    
    public bool Verified { get; set; }
    
    public bool NotificationEnabled { get; set; }
}