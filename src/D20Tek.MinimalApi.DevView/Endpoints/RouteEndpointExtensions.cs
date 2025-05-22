using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace D20Tek.MinimalApi.DevView.Endpoints;

internal static class RouteEndpointExtensions
{
    private const string _noData = "none";

    public static Dictionary<string, object?> InspectEndpoint(this RouteEndpoint endpoint, DevViewOptions options)
    {
        var routePattern = endpoint.GetRoutePattern();
        if (routePattern.StartsWith(options.BasePath)) return [];

        var routeInfo = new Dictionary<string, object?>
        {
            ["Method"] = endpoint.GetHttpMethods(),
            ["Pattern"] = routePattern
        };

        if (options.IncludeRouteMetadata)
        {
            routeInfo["Tags"] = endpoint.GetTags();
            routeInfo["Handler"] = endpoint.RequestDelegate!.Method.Name;
            routeInfo["Name"] = endpoint.GetEndpointName();

            var produces = GetProducesResponses(endpoint);
            if (produces.Length > 0)
            {
                routeInfo["Produces"] = produces;
            }
        }

        if (options.IncludeRouteDebugDetails)
        {
            routeInfo["MetadataTypes"] = endpoint.Metadata.Select(t => $"Type: {t.GetType().Name}, Value:{t}")
                                                          .ToArray();
        }

        return routeInfo;
    }

    private static string GetRoutePattern(this RouteEndpoint endpoint)
    {
        var pattern = endpoint.RoutePattern.RawText;
        return string.IsNullOrEmpty(pattern) ? _noData : pattern;
    }

    private static string[] GetHttpMethods(this RouteEndpoint endpoint) =>
        endpoint.Metadata.OfType<HttpMethodMetadata>()
                         .SelectMany(m => m.HttpMethods)
                         .DefaultIfEmpty(_noData)
                         .ToArray();

    private static string[] GetTags(this RouteEndpoint endpoint) =>
        endpoint.Metadata.OfType<TagsAttribute>()
                         .SelectMany(m => m.Tags)
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
