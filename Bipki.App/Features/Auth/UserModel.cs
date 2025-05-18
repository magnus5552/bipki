namespace Bipki.App.Features.Auth;

public class UserModel
{
    public Guid Id { get; set; }

    public string UserName { get; set; } = null!;

    public string Telegram { get; set; } = null!;

    public string Role { get; set; } = null!;
    
    public Guid? ConferenceId { get; set; }
}