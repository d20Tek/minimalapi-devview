using Microsoft.AspNetCore.Http;

namespace D20Tek.MinimalApi.DevView.Endpoints.Configs;

public sealed class ConfigQuery
{
    public string? KeyName { get; init; }

    public string? Source { get; init; }

    public string? ValueType { get; init; }

    public static ConfigQuery Create(IQueryCollection query)
    {
        ArgumentNullException.ThrowIfNull(query, nameof(query));
        return new()
        {
            KeyName = query["keyName"],
            Source = query["source"],
            ValueType = query["valueType"]
        };
    }

    public IEnumerable<ConfigInfo> ApplyFilters(IEnumerable<ConfigInfo> configs)
    {
        var filtered = configs;

        if (string.IsNullOrWhiteSpace(KeyName) is false)
        {
            filtered = filtered.Where(c => c.Key!.Contains(KeyName, StringComparison.OrdinalIgnoreCase) is true);
        }

        if (string.IsNullOrWhiteSpace(Source) is false)
        {
            filtered = filtered.Where(c => c.Source!.Contains(Source, StringComparison.OrdinalIgnoreCase) is true);
        }

        if (string.IsNullOrWhiteSpace(ValueType) is false)
        {
            filtered = filtered.Where(c =>
                c.ValueType!.Contains(ValueType, StringComparison.OrdinalIgnoreCase) is true);
        }

        return filtered;
    }
}
