namespace Bipki.Database.Models;

public class Chat
{
    public string Title { get; set; } = null!;

    public ChatType Type { get; set; }
    
    public Guid Id { get; set; }
}