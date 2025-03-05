using Microsoft.Extensions.Configuration;

namespace Speech.Demo;

internal class ConfigurationManager
{
    public ConfigurationManager()
    {
        if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")))
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
        }

        IConfiguration config = GetConFiguration();
        var settings = new AppSettings();
        config.GetSection(nameof(AppSettings)).Bind(settings);
        Settings = settings;
    }

    public AppSettings Settings { get; }

    private static IConfigurationRoot GetConFiguration()
    {
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        const string requiredFile = "appsettings.json";
        var optionalFile = $"appsettings.{env}.json";

        return new ConfigurationBuilder()
            .AddJsonFile(requiredFile, false)
            .AddJsonFile(optionalFile, true)
            .Build();
    }
}