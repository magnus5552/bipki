using Bipki.Database.Models.Core;
using Bipki.Database.Models.UserModels;

namespace Bipki.Database.Models.BusinessModels;

public class Vote: Entity
{
    public Guid PollOptionId { get; set; }
    
    public Guid UserId { get; set; }

    public virtual PollOption PollOption { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}