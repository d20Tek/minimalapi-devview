using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Patterns;

namespace D20Tek.MinimalApi.DevView.UnitTests.Fakes;

internal static class EndpointDataSourceFactory
{
    public static RouteEndpoint CreateRouteEndpoint(string method, string route, string handlerDisplayName)
    {
        var metadata = new EndpointMetadataCollection(new HttpMethodMetadata(new[] { method }));
        var routePattern = RoutePatternFactory.Parse(route);
        var pipeline = new RequestDelegate(context => Task.CompletedTask);

        return new RouteEndpoint(pipeline, routePattern, 0, metadata, handlerDisplayName);
    }
}
