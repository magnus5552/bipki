namespace Bipki.App.Features.Admin.Requests;

public class PatchConferenceRequest
{
    public string? Name { get; set; } = null!;
    
    public string? Description { get; set; } = null!;
    
    public DateTime? StartDate { get; set; }
    
    public DateTime? EndDate { get; set; }

    public string? Location { get; set; } = null!;
}