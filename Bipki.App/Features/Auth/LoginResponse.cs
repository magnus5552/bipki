using Bipki.Database.Models;

namespace Bipki.App.Features.Auth;

public class LoginResponse
{
    public User User { get; set; } = null!;
    public string Role { get; set; } = null!;
}