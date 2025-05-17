using Bipki.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Bipki.Database;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabaseServices(this IServiceCollection services, string connectionString) =>
        services
            .AddDbContext<BipkiContext>(options => options.UseNpgsql(connectionString))
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IActivityRepository, ActivityRepository>()
            .AddScoped<IConferenceRepository, ConferenceRepository>();
}