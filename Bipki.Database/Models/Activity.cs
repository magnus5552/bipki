using Bipki.Database.Models.BusinessModels;

namespace Bipki.Database.Models;

public class Activity
{
    public Guid? Id { get; set; }
    
    public Guid ConferenceId { get; set; }
    
    public string Name { get; set; } = null!;
    
    public string Description { get; set; } = null!;
    
    public DateTime StartsAt { get; set; }
    
    public DateTime EndsAt { get; set; }
    
    public ActivityType Type { get; set; }

    public int TotalSeats { get; set; }
    
    public Guid ChatId { get; set; }
    
    public string Recording { get; set; } = null!;
}