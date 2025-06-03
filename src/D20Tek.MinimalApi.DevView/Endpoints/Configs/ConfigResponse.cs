namespace D20Tek.MinimalApi.DevView.Endpoints.Configs;

public sealed record ConfigResponse(ConfigSummary Summary, List<ConfigInfo> ConfigDetails);

public sealed record ConfigSummary(
    string EnvironmentName,
    string[] LoadedJsonFiles,
    ProviderInfo[] Providers,
    string[] EffectiveUrls);

public sealed record ProviderInfo(string Name, int ProvidedKeys);

public sealed record ConfigInfo(string Key, string? Value, string Source, bool IsSensitive, string? ValueType);
