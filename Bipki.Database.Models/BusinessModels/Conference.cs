using Bipki.Database.Models.Core;

namespace Bipki.Database.Models.BusinessModels;

public class Conference : Entity
{
    public string Name { get; set; } = null!;
    
    public string Description { get; set; } = null!;
    
    public string Plan { get; set; } = null!;

    public string Location { get; set; } = null!;
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
    
    public Guid ChatId { get; set; }

    public virtual IEnumerable<Activity> Program { get; set; } = null!;

    public virtual Chat Chat { get; set; } = null!;
}