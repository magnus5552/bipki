using WebPush;

namespace Bipki.App.Features.Notifications;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWebPushServices(this IServiceCollection services)
    {
        services.AddScoped<IWebPushClient>(_ => new WebPushClient());
        return services;
    }
}