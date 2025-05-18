namespace Bipki.Database.Models;

public class Vote
{
    public Guid PollOptionId { get; set; }
    
    public Guid UserId { get; set; }
    
    public Guid Id { get; set; }
}