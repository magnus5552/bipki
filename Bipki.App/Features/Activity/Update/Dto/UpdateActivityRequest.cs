using Bipki.Database.Models.BusinessModels;

namespace Bipki.App.Features.Activity.Update.Dto;

public class UpdateActivityRequest
{
    public string? Name { get; set; }
    
    public string? Description { get; set; }
    
    public DateTime? StartTime { get; set; }
    
    public DateTime? EndTime { get; set; }
    
    public ActivityType? Type { get; set; }

    public int? TotalSeats { get; set; }
    
    public string? Recording { get; set; }
}