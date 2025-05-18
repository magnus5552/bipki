using Bipki.Database.Models.Core;

namespace Bipki.Database.Models.BusinessModels;

public class NotificationSubscription : Entity
{
    public string PushEndpoint { get; set; } = null!;
    public string PushP256DH { get; set; } = null!;
    public string PushAuth { get; set; } = null!;
}