using Bipki.Database.Models.BusinessModels;
using Microsoft.AspNetCore.Identity;

namespace Bipki.Database.Models.UserModels;

public class User: IdentityUser<Guid>
{
    public virtual IEnumerable<ActivityRegistration> ActivityRegistrations { get; set; }
    
    public virtual IEnumerable<WaitListEntry> WaitLists { get; set; }
}