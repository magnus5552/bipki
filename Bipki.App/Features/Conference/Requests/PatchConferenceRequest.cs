namespace Bipki.App.Features.Conference.Requests;

public class PatchConferenceRequest
{
    public string? Name { get; set; }
    
    public string? Description { get; set; }
    
    public string? Plan { get; set; }
    
    public string? Location { get; set; }
    
    public DateTime? StartDate { get; set; }
    
    public DateTime? EndDate { get; set; }
}