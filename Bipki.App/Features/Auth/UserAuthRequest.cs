namespace Bipki.App.Features.Auth;

public class UserAuthRequest
{
    public string Name { get; set; } = null!;
    
    public string Surname { get; set; } = null!;
    
    public string Telegram { get; set; } = null!;
    
    public string Password { get; set; } = null!;
    
    public Guid ConferenceId { get; set; }
} 