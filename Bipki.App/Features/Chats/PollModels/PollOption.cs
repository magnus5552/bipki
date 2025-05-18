namespace Bipki.App.Features.Chats.PollModels;

public class PollOption
{
    public string Text { get; set; } = null!;
    
    public int VotesCount { get; set; }
    
    public bool Selected { get; set; }
    
    public Guid Id { get; set; }
}