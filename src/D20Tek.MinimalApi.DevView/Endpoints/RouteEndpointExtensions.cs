using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace D20Tek.MinimalApi.DevView.Endpoints;

internal static class RouteEndpointExtensions
{
    public static Dictionary<string, object?> InspectEndpoint(this RouteEndpoint endpoint, DevViewOptions options)
    {
        var routePattern = endpoint.RoutePattern.RawText ?? "N/A";
        if (routePattern.StartsWith(options.BasePath)) return [];

        var routeInfo = new Dictionary<string, object?>
        {
            ["Method"] = endpoint.Metadata.OfType<HttpMethodMetadata>().FirstOrDefault()?
                                          .HttpMethods.FirstOrDefault() ?? "N/A",
            ["Pattern"] = routePattern
        };

        if (options.IncludeRouteMetadata)
        {
            routeInfo["Handler"] = endpoint.RequestDelegate?.Method?.Name;

            if (GetProducesResponses(endpoint) is { Length: > 0 } produces)
            {
                routeInfo["Produces"] = produces;
            }

            if (options.IncludeRouteDebugDetails)
            {
                routeInfo["MetadataTypes"] = endpoint.Metadata.Select(t => $"Type: {t.GetType().Name}, Data:{t}");
            }
        }

        return routeInfo;
    }

    private static string[] GetProducesResponses(RouteEndpoint endpoint) =>
        endpoint.Metadata.OfType<ProducesAttribute>()
                         .Select(attr => attr.ToString() ?? "")
                         .Concat(endpoint.Metadata
                                         .OfType<ProducesResponseTypeMetadata>()
                                         .Select(meta => meta.ToString()))
                         .Distinct()
                         .ToArray();

    internal record MetadataSerialized(string Type, string Data);
}
