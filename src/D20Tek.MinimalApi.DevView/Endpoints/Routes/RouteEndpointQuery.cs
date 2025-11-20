using Microsoft.AspNetCore.Http;

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

    public IEnumerable<Dictionary<string, object?>> ApplyFilters(IEnumerable<Dictionary<string, object?>> endpoints)
    {
        var filtered = endpoints;
        if (string.IsNullOrWhiteSpace(Route) is false)
        {
            filtered = filtered.Where(
                d => DictionaryItemAsString(d, _patternFilter).Contains(Route, StringComparison.OrdinalIgnoreCase));
        }

        if (string.IsNullOrWhiteSpace(Method) is false)
        {
            filtered = filtered.Where(
                d => DictionaryItemAsStringArray(d, _methodFilter).Contains(Method.ToUpper()));
        }

        if (string.IsNullOrWhiteSpace(EndpointName) is false)
        {
            filtered = filtered.Where(
                d => DictionaryItemAsString(d, _nameFilter).Contains(EndpointName, StringComparison.OrdinalIgnoreCase));
        }

        if (string.IsNullOrWhiteSpace(Tag) is false)
        {
            filtered = filtered.Where(
                d => DictionaryItemAsStringArray(d, _tagsFilter).Contains(Tag));
        }

        return filtered;
    }

    private static string DictionaryItemAsString(Dictionary<string, object?> dict, string key) =>
        dict.TryGetValue(key, out var value) && value is string result ? result : string.Empty;

    private static string[] DictionaryItemAsStringArray(Dictionary<string, object?> dict, string key) =>
        dict.TryGetValue(key, out var value) && value is string[] result ? result : [];
}
