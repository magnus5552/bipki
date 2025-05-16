using Bipki.Database.Models.BusinessModels;
using Bipki.Database.Models.UserModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bipki.Database;

public class BipkiContext : IdentityDbContext<User, Role, Guid>
{
    public BipkiContext(DbContextOptions<BipkiContext> options)
        : base(options)
    {
    }
    
    public virtual DbSet<Conference> Conferences { get; set; }
    
    public virtual DbSet<Activity> Activities { get; set; }

    public virtual DbSet<Location> Locations { get; set; }
    
    public virtual DbSet<ActivityRegistration> ActivityRegistrations { get; set; }
    
    public virtual DbSet<WaitListEntry> WaitListEntries { get; set; }
    
    public virtual DbSet<User> Users { get; set; }
}