using Bipki.Database.Models.BusinessModels;

namespace Bipki.App.Features.Activity.Create.Dto;

public class CreateActivityRequest
{
    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public ActivityType Type { get; set; }
    
    public int TotalSeats { get; set; }
}