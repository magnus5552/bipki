namespace Bipki.App.Features.Conference.Requests;

public class CreateConferenceRequest
{
    public string Name { get; set; } = null!;
    
    public string Description { get; set; } = null!;

    public string Plan { get; set; } = null!;
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }

    public string Location { get; set; } = null!;
}