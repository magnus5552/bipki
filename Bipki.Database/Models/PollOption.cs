namespace Bipki.Database.Models;

public class PollOption
{
    public Guid PollId { get; set; }

    public string Text { get; set; } = null!;
    
    public Guid Id { get; set; }
}