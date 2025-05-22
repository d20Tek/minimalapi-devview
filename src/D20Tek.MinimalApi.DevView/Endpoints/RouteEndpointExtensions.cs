using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace D20Tek.MinimalApi.DevView.Endpoints;

internal static class RouteEndpointExtensions
{
    private const string _noData = "none";

    public static Dictionary<string, object?> InspectEndpoint(this RouteEndpoint endpoint, DevViewOptions options)
    {
        var routePattern = endpoint.GetRoutePattern() ?? _noData;
        if (routePattern.StartsWith(options.BasePath)) return [];

        var routeInfo = new Dictionary<string, object?>
        {
            ["Method"] = endpoint.GetHttpMethods(),
            ["Pattern"] = routePattern
        };

        if (options.IncludeRouteMetadata)
        {
            routeInfo["Handler"] = endpoint.RequestDelegate?.Method?.Name;
            routeInfo["Name"] = endpoint.GetEndpointName();

            if (GetProducesResponses(endpoint) is { Length: > 0 } produces)
            {
                routeInfo["Produces"] = produces;
            }

        }

        if (options.IncludeRouteDebugDetails)
        {
            routeInfo["MetadataTypes"] = endpoint.Metadata.Select(t => $"Type: {t.GetType().Name}, Value:{t}");
        }

        return routeInfo;
    }

    private static string? GetRoutePattern(this RouteEndpoint endpoint) => endpoint.RoutePattern.RawText;

    private static string[] GetHttpMethods(this RouteEndpoint endpoint) =>
        endpoint.Metadata.OfType<HttpMethodMetadata>()
                         .SelectMany(m => m.HttpMethods)
                         .DefaultIfEmpty(_noData)
                         .ToArray();

    private static string? GetEndpointName(this RouteEndpoint endpoint) =>
        endpoint.Metadata.OfType<EndpointNameMetadata>()
                         .FirstOrDefault()?
                         .EndpointName;

    private static string[] GetProducesResponses(RouteEndpoint endpoint) =>
        endpoint.Metadata.OfType<ProducesResponseTypeMetadata>()
                         .GroupBy(meta => new
                         {
                            meta.StatusCode,
                            Key = string.Join(",", meta.ContentTypes.OrderBy(c => c))
                         })
                         // Prefer the more specific ResponseType (i.e., not IResult)
                         .Select(g => g.FirstOrDefault(x => x.Type != typeof(IResult)) ?? g.First())
                         .Select(meta => FormatProducesMetadata(meta))
                         .ToArray();

    private static string FormatProducesMetadata(ProducesResponseTypeMetadata meta) =>
        $"StatusCode: {meta.StatusCode}, ContentTypes: {string.Join(";", meta.ContentTypes)}, Type: {meta.Type}";
}
