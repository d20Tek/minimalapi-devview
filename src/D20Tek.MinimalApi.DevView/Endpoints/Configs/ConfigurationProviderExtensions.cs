using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration;

namespace D20Tek.MinimalApi.DevView.Endpoints.Configs;

internal static class ConfigurationProviderExtensions
{
    private static string _appSettings(string? file) => $"appsettings ({file})";
    private const string _secretsProvider = "UserSecretsConfigurationProvider";
    private const string _userSecrets = "User Secrets";
    private const string _env = "Environment Variable";
    private const string _commandLine = "Command Line";
    private const string _inMemory = "In-Memory";

    public static string GetFriendlyName(this IConfigurationProvider provider) =>
        provider switch
        {
            JsonConfigurationProvider json => _appSettings(json.Source.Path),

            _ when provider.GetType().FullName?.Contains(_secretsProvider) == true => _userSecrets,

            Microsoft.Extensions.Configuration.EnvironmentVariables.EnvironmentVariablesConfigurationProvider => _env,

            Microsoft.Extensions.Configuration.CommandLine.CommandLineConfigurationProvider => _commandLine,

            Microsoft.Extensions.Configuration.Memory.MemoryConfigurationProvider => _inMemory,

            _ => provider.GetType().Name
        };
}
