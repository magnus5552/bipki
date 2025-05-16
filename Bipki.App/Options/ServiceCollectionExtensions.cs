namespace Bipki.App.Options;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAllOptions(this IServiceCollection services)
    {
        services
            .Configure<ApplicationOptions>(ApplicationOptionsProvider.GetConfiguration());
        services
            .Configure<AuthorizationOptions>(ApplicationOptionsProvider.GetConfiguration()
                .GetSection(nameof(AuthorizationOptions)));
        services
            .Configure<DatabaseOptions>(ApplicationOptionsProvider.GetConfiguration()
                .GetSection(nameof(DatabaseOptions)));

        return services;
    }
}