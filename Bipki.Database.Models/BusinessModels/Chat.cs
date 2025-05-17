using Bipki.Database.Models.Core;

namespace Bipki.Database.Models.BusinessModels;

public class Chat : Entity
{
    public string Title { get; set; } = null!;

    public ChatType Type { get; set; }
    
    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
}