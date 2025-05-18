namespace Bipki.App.Features.Chats.PollModels;

public class Poll
{
    public Guid Id { get; set; }

    public string Text { get; set; } = null!;

    public string SenderName { get; set; } = null!;
    
    public DateTime TimeStamp { get; set; }
    
    public int TotalVotesCount { get; set; }

    public ICollection<PollOption> Options { get; set; } = new List<PollOption>();
}