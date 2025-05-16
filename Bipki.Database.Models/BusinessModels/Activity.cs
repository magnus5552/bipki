using Bipki.Database.Models.Core;

namespace Bipki.Database.Models.BusinessModels;

public class Activity : Entity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime StartsAt { get; set; }
    public DateTime EndsAT { get; set; }
    public ActivityType Type { get; set; }
    // TODO communication channel
    public string Recording { get; set; }
}