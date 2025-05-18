using WebPush;

namespace Bipki.App.Features.Notifications;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWebPushServices(this IServiceCollection services)
    {
        services.AddSingleton(() => new VapidDetails("mailto:idi@nahuy.pidor", "BHsQmzWZk93Nctx0xByvPqhYoFaiO7vT-lwOHVjNFKymeDz-cl5T-NnSlTm_zj7emq9bjYqIWiD14bXQOotYz5U", "1h-76WbxIy9wGwXO8IHtCvfFC4cMlm8Z4m4-KViTkvs"));
        services.AddScoped<IWebPushClient>(_ => new WebPushClient());
        services.AddScoped<NotificationsManager>();
        return services;
    }
}