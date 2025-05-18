namespace Bipki.Database.Models;

public class Conference
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = null!;
    
    public string Description { get; set; } = null!;
    
    public string Plan { get; set; } = null!;
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }

    public string Location { get; set; } = null!;
    
    public Guid ChatId { get; set; }
}