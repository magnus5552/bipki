namespace Bipki.Database.Models;

public class Conference
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = null!;
    
    public string Description { get; set; } = null!;
    
    public string Plan { get; set; } = null!;
    
    public Guid ActivityId { get; set; }
    
    public Guid LocationId { get; set; }
}