namespace D20Tek.MinimalApi.DevView.Endpoints.Routes;

public sealed class RouteEndpointQuery
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
                d => d.ItemAsString(_patternFilter).Contains(Route!, StringComparison.OrdinalIgnoreCase))
            .ApplyWhereIf(
                Method,
                d => d.ItemAsStringArray(_methodFilter).Contains(Method!.ToUpper()))
            .ApplyWhereIf(
                EndpointName,
                d => d.ItemAsString(_nameFilter).Contains(EndpointName!, StringComparison.OrdinalIgnoreCase))
            .ApplyWhereIf(
                Tag,
                d => d.ItemAsStringArray(_tagsFilter).Contains(Tag));
}
