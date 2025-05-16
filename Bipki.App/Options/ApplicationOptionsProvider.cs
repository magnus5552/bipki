namespace Bipki.App.Options;

public static class ApplicationOptionsProvider
{
    public static IConfigurationRoot GetConfiguration()
    {
        var path = AppDomain.CurrentDomain.BaseDirectory;
        var configurationBuilder = new ConfigurationBuilder().SetBasePath(path);
        configurationBuilder.AddEnvironmentVariables();
        configurationBuilder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        return configurationBuilder.Build();
    }
}