using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration;

namespace D20Tek.MinimalApi.DevView.Endpoints.Configs;

internal static class ConfigurationProviderExtensions
{
    public static string GetFriendlyName(this IConfigurationProvider provider) =>
        provider switch
        {
            JsonConfigurationProvider json => $"appsettings ({json.Source.Path})",

            _ when provider.GetType().FullName?.Contains("UserSecretsConfigurationProvider") == true =>
                "User Secrets",

            Microsoft.Extensions.Configuration.EnvironmentVariables.EnvironmentVariablesConfigurationProvider =>
                "Environment Variable",

            Microsoft.Extensions.Configuration.CommandLine.CommandLineConfigurationProvider =>
                "Command Line",

            Microsoft.Extensions.Configuration.Memory.MemoryConfigurationProvider =>
                "In-Memory",

            _ => provider.GetType().Name
        };
}
