namespace Bipki.App.Features.Chats;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddChats(this IServiceCollection services)
    {
        services.AddSignalR();

        return services;
    }
}