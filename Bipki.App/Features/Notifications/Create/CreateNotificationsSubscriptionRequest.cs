namespace Bipki.App.Features.Notifications.Create;

public class CreateNotificationsSubscriptionRequest
{
    public string PushEndpoint { get; set; } = null!;
    public string PushP256DH { get; set; } = null!;
    public string PushAuth { get; set; } = null!;
}