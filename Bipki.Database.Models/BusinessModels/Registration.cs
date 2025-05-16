using Bipki.Database.Models.UserModels;

namespace Bipki.Database.Models.BusinessModels;

public class ActivityRegistration
{
    public Guid ActivityId { get; set; }
    public Guid UserId { get; set; }
    public DateTime RegistereAt { get; set; }
    public bool Verified { get; set; }
    
    public virtual Activity Activity { get; set; }
    public virtual User User { get; set; }
}