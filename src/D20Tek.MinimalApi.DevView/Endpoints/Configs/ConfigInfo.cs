namespace D20Tek.MinimalApi.DevView.Endpoints.Configs;

public sealed class ConfigInfo
{
    public string Key { get; init; } = string.Empty;

    public string? Value { get; init; }

    public string Source { get; init; } = string.Empty;

    public bool IsSensitive { get; init; }

    public string? ValueType { get; init; }
}