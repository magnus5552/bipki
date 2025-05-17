using Bipki.Database.Models.BusinessModels;
using Microsoft.AspNetCore.Identity;

namespace Bipki.Database.Models.UserModels;

public class User: IdentityUser<Guid>
{
    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string Telegram { get; set; } = null!;

    public Guid? ConferenceId { get; set; }
    
    public bool CheckedIn { get; set; }
    
    public virtual IEnumerable<ActivityRegistration> ActivityRegistrations { get; set; } = null!;

    public virtual IEnumerable<WaitListEntry> WaitLists { get; set; } = null!;
}