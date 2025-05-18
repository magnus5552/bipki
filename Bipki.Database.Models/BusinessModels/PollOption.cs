using Bipki.Database.Models.Core;

namespace Bipki.Database.Models.BusinessModels;

public class PollOption: Entity
{
    public Guid PollId { get; set; }

    public string Text { get; set; } = null!;

    public virtual Poll Poll { get; set; } = null!;

    public virtual ICollection<Vote> Votes { get; set; } = new List<Vote>();
}