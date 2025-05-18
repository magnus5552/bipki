namespace Bipki.Database.Models;

public class NotificationSubscription
{
    public Guid Id { get; set; }
    public string PushEndpoint { get; set; } = null!;
    public string PushP256DH { get; set; } = null!;
    public string PushAuth { get; set; } = null!;
}