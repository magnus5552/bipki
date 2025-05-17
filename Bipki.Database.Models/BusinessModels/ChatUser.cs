using Bipki.Database.Models.UserModels;

namespace Bipki.Database.Models.BusinessModels;

public class ChatUser
{
    public Guid UserId { get; set; }
    
    public Guid ChatId { get; set; }

    public virtual User User { get; set; } = null!;

    public virtual Chat Chat { get; set; } = null!;
}