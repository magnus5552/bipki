using Bipki.Database.Models.UserModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Bipki.Database;

public class BipkiContext : IdentityDbContext<User, Role, Guid>
{
    
}