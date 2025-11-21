namespace D20Tek.MinimalApi.DevView.Endpoints.Routes;

public class RouteEndpointQuery
{
    private const string _routeKey = "route";
    private const string _methodKey = "method";
    private const string _endpointNameKey = "endpointName";
    private const string _tagKey = "tag";
    private const string _patternFilter = "Pattern";
    private const string _methodFilter = "Method";
    private const string _nameFilter = "Name";
    private const string _tagsFilter = "Tags";

    public string? Route { get; init; }

    public string? Method { get; init; }

    public string? EndpointName { get; init; }

    public string? Tag { get; init; }

    public static RouteEndpointQuery Create(IQueryCollection query)
    {
        ArgumentNullException.ThrowIfNull(query, nameof(query));
        return new()
        {
            Route = query[_routeKey],
            Method = query[_methodKey],
            EndpointName = query[_endpointNameKey],
            Tag = query[_tagKey]
        };
    }

    public IEnumerable<Dictionary<string, object?>> ApplyFilters(IEnumerable<Dictionary<string, object?>> endpoints) =>
        endpoints
            .ApplyWhereIf(
                Route,
                d => DictionaryItemAsString(d, _patternFilter).Contains(Route!, StringComparison.OrdinalIgnoreCase))
            .ApplyWhereIf(
                Method,
                d => DictionaryItemAsStringArray(d, _methodFilter).Contains(Method!.ToUpper()))
            .ApplyWhereIf(
                EndpointName,
                d => DictionaryItemAsString(d, _nameFilter).Contains(EndpointName!, StringComparison.OrdinalIgnoreCase))
            .ApplyWhereIf(
                Tag,
                d => DictionaryItemAsStringArray(d, _tagsFilter).Contains(Tag));

    private static string DictionaryItemAsString(Dictionary<string, object?> dict, string key) =>
        dict.TryGetValue(key, out var value) && value is string result ? result : string.Empty;

    private static string[] DictionaryItemAsStringArray(Dictionary<string, object?> dict, string key) =>
        dict.TryGetValue(key, out var value) && value is string[] result ? result : [];
}
