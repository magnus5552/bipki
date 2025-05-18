namespace Bipki.App.Features.Activity;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddActivitiesServices(this IServiceCollection services)
    {
        services.AddScoped<ZalupaService>();
        return services;
    }
}