using Bipki.Database.Models.Core;
using Bipki.Database.Models.UserModels;

namespace Bipki.Database.Models.BusinessModels;

public class WaitList : Entity
{
    public Guid ActivityId { get; set; }
    public Guid UserId { get; set; }
    public DateTime WaitsSince { get; set; }
    
    public virtual Activity Activity { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}