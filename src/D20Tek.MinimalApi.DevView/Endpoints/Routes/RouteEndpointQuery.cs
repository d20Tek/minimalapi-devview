using Microsoft.AspNetCore.Http;

namespace D20Tek.MinimalApi.DevView.Endpoints.Routes;

public class RouteEndpointQuery
{
    public string? Route { get; init; }

    public string? Method { get; init; }

    public string? EndpointName { get; init; }

    public string? Tag { get; init; }

    public static RouteEndpointQuery Create(IQueryCollection query)
    {
        ArgumentNullException.ThrowIfNull(query, nameof(query));
        return new()
        {
            Route = query["route"],
            Method = query["method"],
            EndpointName = query["endpointName"],
            Tag = query["tag"]
        };
    }

    public IEnumerable<Dictionary<string, object?>> ApplyFilters(IEnumerable<Dictionary<string, object?>> endpoints)
    {
        var filtered = endpoints;
        if (string.IsNullOrWhiteSpace(Route) is false)
        {
            filtered = filtered.Where(
                d => DictionaryItemAsString(d, "Pattern").Contains(Route));
        }

        if (string.IsNullOrWhiteSpace(Method) is false)
        {
            filtered = filtered.Where(
                d => DictionaryItemAsStringArray(d, "Method").Contains(Method));
        }

        if (string.IsNullOrWhiteSpace(EndpointName) is false)
        {
            filtered = filtered.Where(
                d => DictionaryItemAsString(d, "Name").Contains(EndpointName));
        }

        if (string.IsNullOrWhiteSpace(Tag) is false)
        {
            filtered = filtered.Where(
                d => DictionaryItemAsStringArray(d, "Tags").Contains(Tag));
        }

        return filtered;
    }

    private static string DictionaryItemAsString(Dictionary<string, object?> dict, string key) =>
        dict[key] as string ?? string.Empty;

    private static string[] DictionaryItemAsStringArray(Dictionary<string, object?> dict, string key) =>
        dict[key] as string[] ?? [string.Empty];
}
