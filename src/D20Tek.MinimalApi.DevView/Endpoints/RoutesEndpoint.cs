using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
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
                      .Select(ep => ep.InspectEndpoint(options))
                      .Where(x => x.Count > 0);
}
