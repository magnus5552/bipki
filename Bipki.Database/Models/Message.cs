namespace Bipki.Database.Models;

public class Message
{
    public DateTime Timestamp { get; set; }

    public string Text { get; set; } = null!;
    
    public Guid ChatId { get; set; }
    
    public Guid SenderId { get; set; }

    public string SenderName { get; set; } = null!;
    
    public Guid Id { get; set; }
}