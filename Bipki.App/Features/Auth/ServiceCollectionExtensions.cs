using Bipki.Database;
using Bipki.Database.Models.UserModels;
using Microsoft.AspNetCore.Identity;

namespace Bipki.App.Features.Auth;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services)
    {
        services.AddIdentity<User, Role>()
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<BipkiContext>();

        services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.HttpOnly = true;
            options.AccessDeniedPath = "/api/auth/access-denied";
        });
        return services;
    }
}