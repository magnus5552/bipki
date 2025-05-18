using Bipki.Database.Models.Core;

namespace Bipki.Database.Models.BusinessModels;

public class Chat : Entity
{
    public string Title { get; set; } = null!;

    public ChatType Type { get; set; }
    
    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual ICollection<Poll> Polls { get; set; } = new List<Poll>();

    public virtual Activity Activity { get; set; } = null!;

    public virtual Conference Conference { get; set; } = null!;
}