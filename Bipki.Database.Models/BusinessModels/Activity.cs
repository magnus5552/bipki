using Bipki.Database.Models.Core;

namespace Bipki.Database.Models.BusinessModels;

public class Activity : Entity
{
    public string Name { get; set; } = null!;
    
    public string Description { get; set; } = null!;
    
    public DateTime StartsAt { get; set; }
    
    public DateTime EndsAt { get; set; }
    
    public ActivityType Type { get; set; }
    
    public int TotalParticipants { get; set; }
    
    public Guid ChatId { get; set; }
    
    // TODO communication channel
    public string Recording { get; set; } = null!;

    public virtual IEnumerable<ActivityRegistration> ActivityRegistrations { get; set; } = null!;

    public virtual Chat Chat { get; set; } = null!;

    public virtual IEnumerable<WaitListEntry> WaitList { get; set; } = null!;
}