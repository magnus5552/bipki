namespace Bipki.Database.Models;

public class Poll
{
    public Guid ChatId { get; set; }
    
    public DateTime TimeStamp { get; set; }

    public string Title { get; set; } = null!;
    
    public Guid Id { get; set; }
}