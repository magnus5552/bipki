using Bipki.Database.Models.UserModels;

namespace Bipki.App.Features.Auth;

public class LoginResponse
{
    public User User { get; set; } = null!;
    public string Role { get; set; } = null!;
}