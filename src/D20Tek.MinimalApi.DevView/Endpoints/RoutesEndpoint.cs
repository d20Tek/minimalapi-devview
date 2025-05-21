using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace D20Tek.MinimalApi.DevView.Endpoints;

public static class RoutesEndpoint
{
    public static IEndpointRouteBuilder MapRoutesExplorer(this IEndpointRouteBuilder endpoints, DevViewOptions options)
    {
        var path = $"{options.BasePath?.TrimEnd('/')}/routes";

        endpoints.MapGet(path, (EndpointDataSource endpointSource) =>
        {
            ArgumentNullException.ThrowIfNull(endpointSource, nameof(endpointSource));
            var routes = DiscoverRoutes(endpointSource, options);
            return Results.Json(routes);
        });

        return endpoints;
    }

    private static IEnumerable<Dictionary<string, object?>> DiscoverRoutes(
        EndpointDataSource endpointSource,
        DevViewOptions options) =>
        endpointSource.Endpoints
                      .OfType<RouteEndpoint>()
                      .Select(ep => InspectEndpoint(ep, options));

    private static Dictionary<string, object?> InspectEndpoint(RouteEndpoint endpoint, DevViewOptions options)
    {
        var routeInfo = new Dictionary<string, object?>
        {
            ["Method"] = endpoint.Metadata.OfType<HttpMethodMetadata>().FirstOrDefault()?
                                          .HttpMethods.FirstOrDefault() ?? "N/A",
            ["Pattern"] = endpoint.RoutePattern.RawText ?? "N/A"
        };

        if (options.IncludeRouteMetadata)
        {
            routeInfo["Handler"] = endpoint.RequestDelegate?.Method?.Name;

            if (GetProducesResponses(endpoint) is { Length: > 0 } produces)
            {
                routeInfo["Produces"] = produces;
            }
        }

        return routeInfo;
    }

    private static string[] GetProducesResponses(RouteEndpoint endpoint) =>
        endpoint.Metadata.OfType<ProducesAttribute>()
                         .SelectMany(attr => attr.ContentTypes)
                         .Concat(endpoint.Metadata
                                         .OfType<ProducesResponseTypeMetadata>()
                                         .SelectMany(meta => meta.ContentTypes))
                         .Distinct()
                         .ToArray();
}
