namespace Bipki.App.Options;

public class ApplicationOptions
{
     public DatabaseOptions DatabaseOptions { get; set; } = null!;

     public AuthorizationOptions AuthorizationOptions { get; set; } = null!;
}