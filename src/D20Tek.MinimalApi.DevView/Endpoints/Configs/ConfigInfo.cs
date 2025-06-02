namespace D20Tek.MinimalApi.DevView.Endpoints.Configs;

public sealed record ConfigInfo(
    string Key,
    string? Value,
    string Source,
    bool IsOverridden,
    bool IsSensitive,
    string? ValueType);
