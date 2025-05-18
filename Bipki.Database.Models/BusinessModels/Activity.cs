using Bipki.Database.Models.Core;

namespace Bipki.Database.Models.BusinessModels;

public class Activity : Entity
{
    public Guid ConferenceId { get; set; }
    
    public string Name { get; set; } = null!;

    public string TypeLabel { get; set; } = null!;
    
    public string Description { get; set; } = null!;
    
    public DateTime StartsAt { get; set; }
    
    public DateTime EndsAt { get; set; }
    
    public ActivityType Type { get; set; }
    
    public int TotalParticipants { get; set; }
    
    public Guid ChatId { get; set; }
    
    public string Recording { get; set; } = null!;
    
    public virtual Conference Conference { get; set; }
    
    public virtual Chat Chat { get; set; } = null!;

    public virtual IEnumerable<ActivityRegistration> ActivityRegistrations { get; set; }

    public virtual IEnumerable<WaitListEntry> WaitList { get; set; } = null!;
}