using Microsoft.Extensions.Configuration;

namespace Bipki.App.Options;

public static class ApplicationOptionsProvider
{
    public static IConfigurationRoot GetConfiguration()
    {
        var path = AppDomain.CurrentDomain.BaseDirectory;
        var configurationBuilder = new ConfigurationBuilder().SetBasePath(path);
        configurationBuilder.AddEnvironmentVariables();
        
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
        configurationBuilder.AddJsonFile(
            environment == "Development" ? "appsettings.Development.json" : "appsettings.json", optional: false,
            reloadOnChange: true);

        return configurationBuilder.Build();
    }
}