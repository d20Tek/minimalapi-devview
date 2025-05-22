using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;

namespace D20Tek.MinimalApi.DevView.Endpoints;

public static class RoutesEndpoint
{
    public static IEndpointRouteBuilder MapRoutesExplorer(this IEndpointRouteBuilder endpoints, DevViewOptions options)
    {
        var basePath = options.BasePath;
        endpoints.MapGet($"{basePath}/routes", GetRoutes);

        return endpoints;
    }

    internal static IResult GetRoutes(EndpointDataSource endpointSource, IOptions<DevViewOptions> options)
    {
        ArgumentNullException.ThrowIfNull(endpointSource, nameof(endpointSource));
        var routes = DiscoverRoutes(endpointSource, options.Value);
        return Results.Json(routes);
    }

    private static IEnumerable<Dictionary<string, object?>> DiscoverRoutes(
        EndpointDataSource endpointSource,
        DevViewOptions options) =>
        endpointSource.Endpoints.OfType<RouteEndpoint>()
                                .Select(ep => ep.InspectEndpoint(options))
                                .Where(x => x.Count > 0);
}
