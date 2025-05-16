using Bipki.Database.Models.UserModels;

namespace Bipki.Database.Models.BusinessModels;

public class WaitList
{
    public Guid ActivityId { get; set; }
    public Guid UserId { get; set; }
    public DateTime WaitsSince { get; set; }
    
    public virtual Activity Activity { get; set; }
    public virtual User User { get; set; }
}