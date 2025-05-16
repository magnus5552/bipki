using Bipki.Database.Models.Core;

namespace Bipki.Database.Models.BusinessModels;

public class Location : Entity
{
    // TODO
    public virtual IEnumerable<Conference> Conferences { get; set; } = null!;
}