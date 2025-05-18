using Bipki.Database.Models.Core;

namespace Bipki.Database.Models.BusinessModels;

public class Poll: Entity
{
    public Guid ChatId { get; set; }
    
    public DateTime TimeStamp { get; set; }

    public string Title { get; set; } = null!;

    public virtual Chat Chat { get; set; } = null!;

    public virtual ICollection<PollOption> PollOptions { get; set; } = new List<PollOption>();
}