using Bipki.Database.Models.Core;
using Bipki.Database.Models.UserModels;

namespace Bipki.Database.Models.BusinessModels;

public class NotificationSubscription : Entity
{
    public string PushEndpoint { get; set; } = null!;
    public string PushP256DH { get; set; } = null!;
    public string PushAuth { get; set; } = null!;
    public Guid SubscribentId { get; set; }

    public virtual User Subscribent { get; set; } = null!;
}