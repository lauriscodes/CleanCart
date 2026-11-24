using Infrastructure.Helpers;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Extensions;

public static class ConfigurationBuilderExtensions
{
    public static IConfigurationBuilder AddCoreLayerConfiguration(this IConfigurationBuilder configurationBuilder)
    {
        var environment = AspNetEnvironmentHelper.GetRequiredEnvironmentName();
        configurationBuilder.AddJsonFile("appsettings.core.json", false);
        configurationBuilder.AddJsonFile($"appsettings.core.{environment}.json", false);
        configurationBuilder.AddEnvironmentVariables();

        return configurationBuilder;
    }
}