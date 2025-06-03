using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.EnvironmentVariables;

namespace D20Tek.MinimalApi.DevView.Endpoints.Configs;

internal static class ConfigurationRootExtensions
{
    private const string _privateData = "*****";

    public static IList<ConfigInfo> GetConfigDetails(
        this IConfigurationRoot root,
        List<string> keys,
        DevViewOptions options)
    {
        var entries = new Dictionary<string, ConfigInfo>(StringComparer.OrdinalIgnoreCase);
        var providers = root.Providers.ToList();

        foreach (var provider in providers)
        {
            foreach (var key in keys)
            {
                if (!provider.TryGet(key, out var value))
                    continue;

                if (ShouldSkipEnvVariable(provider, key, options))
                    continue;

                var isSensitive = KeywordChecker.IsSensitive(key);

                entries[key] = new ConfigInfo
                {
                    Key = key,
                    Value = isSensitive ? _privateData : value,
                    Source = provider.GetFriendlyName(),
                    IsSensitive = isSensitive,
                    ValueType = ValueTypeMapper.InferFrom(value),
                };
            }
        }

        return entries.Values.OrderBy(e => e.Source)
                             .ThenBy(e => e.Key)
                             .ToList();
    }

    private static bool ShouldSkipEnvVariable(IConfigurationProvider provider, string key, DevViewOptions options) =>
        !options.IncludeAllEnvVariables &&
        provider is EnvironmentVariablesConfigurationProvider &&
        !EnvironmentVariables.IsRelevant(key);
}
