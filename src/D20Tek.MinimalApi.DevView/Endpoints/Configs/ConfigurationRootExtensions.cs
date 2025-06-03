using Microsoft.Extensions.Configuration;

namespace D20Tek.MinimalApi.DevView.Endpoints.Configs;

internal static class ConfigurationRootExtensions
{
    public static IList<ConfigInfo> GetConfigDetails(this IConfigurationRoot root, List<string> keys)
    {
        var entries = new Dictionary<string, ConfigInfo>(StringComparer.OrdinalIgnoreCase);

        var providers = root.Providers.ToList();

        foreach (var provider in providers)
        {
            foreach (var key in keys)
            {
                if (!provider.TryGet(key, out var value))
                    continue;

                var isSensitive = KeywordChecker.IsSensitive(value);

                // Always overwrite with higher-priority provider
                entries[key] = new ConfigInfo
                {
                    Key = key,
                    Value = isSensitive ? "*****" : value,
                    Source = provider.GetFriendlyName(),
                    IsSensitive = isSensitive,
                    ValueType = ValueTypeMapper.InferFrom(value),
                };
            }
        }

        return entries.Values.OrderBy(e => e.Source).ThenBy(e => e.Key).ToList();
    }
}
