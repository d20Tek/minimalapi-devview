namespace D20Tek.MinimalApi.DevView.Endpoints.Configs;

public sealed class ConfigQuery
{
    private const string _keyNameKey = "keyName";
    private const string _sourceKey = "source";
    private const string _valueTypeKey = "valueType";

    public string? KeyName { get; init; }

    public string? Source { get; init; }

    public string? ValueType { get; init; }

    public static ConfigQuery Create(IQueryCollection query)
    {
        ArgumentNullException.ThrowIfNull(query, nameof(query));
        return new()
        {
            KeyName = query[_keyNameKey],
            Source = query[_sourceKey],
            ValueType = query[_valueTypeKey]
        };
    }

    public IEnumerable<ConfigInfo> ApplyFilters(IEnumerable<ConfigInfo> configs) =>
        configs.ApplyWhereIf(KeyName, c => c.Key!.Contains(KeyName!, StringComparison.OrdinalIgnoreCase))
               .ApplyWhereIf(Source, c => c.Source!.Contains(Source!, StringComparison.OrdinalIgnoreCase))
               .ApplyWhereIf(ValueType, c => c.ValueType!.Contains(ValueType!, StringComparison.OrdinalIgnoreCase));
}
