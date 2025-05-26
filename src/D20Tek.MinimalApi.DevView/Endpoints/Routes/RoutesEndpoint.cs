using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;

namespace D20Tek.MinimalApi.DevView.Endpoints.Routes;

public static class RoutesEndpoint
{
    public static IEndpointRouteBuilder MapRoutesExplorer(this IEndpointRouteBuilder endpoints, DevViewOptions options)
    {
        var basePath = options.BasePath;
        endpoints.MapGet($"{basePath}/routes", GetRoutes);

        return endpoints;
    }

    internal static IResult GetRoutes(
        EndpointDataSource endpointSource,
        HttpContext context,
        IOptions<DevViewOptions> options)
    {
        ArgumentNullException.ThrowIfNull(endpointSource, nameof(endpointSource));

        var query = RouteEndpointQuery.Create(context.Request.Query);
        var routes = DiscoverRoutes(endpointSource, options.Value, query);
        return Results.Json(routes);
    }

    private static IEnumerable<Dictionary<string, object?>> DiscoverRoutes(
        EndpointDataSource endpointSource,
        DevViewOptions options,
        RouteEndpointQuery query) =>
        endpointSource.Endpoints.OfType<RouteEndpoint>()
                                .Select(ep => ep.InspectEndpoint(options))
                                .Where(x => x.Count > 0)
                                .Filter(query);

    private static IEnumerable<Dictionary<string, object?>> Filter(
        this IEnumerable<Dictionary<string, object?>> endpoints,
        RouteEndpointQuery query) =>
        query.ApplyFilters(endpoints);
}
