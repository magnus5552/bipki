namespace Bipki.App.Features.Auth;

public class UserLoginRequest
{
    public string Telegram { get; set; } = null!;

    public string Password { get; set; } = null!;
    
    public Guid ConferenceId { get; set; }
}