using Bipki.Database.Models.Core;
using Bipki.Database.Models.UserModels;

namespace Bipki.Database.Models.BusinessModels;

public class ActivityRegistration : Entity
{
    public Guid ActivityId { get; set; }
    public Guid UserId { get; set; }
    public DateTime RegisteredAt { get; set; }
    public bool Verified { get; set; }
    
    public virtual Activity Activity { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}