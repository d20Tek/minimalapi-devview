using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Patterns;
using System.Diagnostics.CodeAnalysis;

namespace D20Tek.MinimalApi.DevView.UnitTests.Fakes;

internal static class EndpointDataSourceFactory
{
    public static RouteEndpoint CreateRouteEndpoint(string method, string route, string handlerDisplayName)
    {
        var metadata = new EndpointMetadataCollection(
            new HttpMethodMetadata(new[] { method }));

        var routePattern = RoutePatternFactory.Parse(route);
        var pipeline = new RequestDelegate([ExcludeFromCodeCoverage] (context) => Task.CompletedTask);

        return new RouteEndpoint(pipeline, routePattern, 0, metadata, handlerDisplayName);
    }

    public static RouteEndpoint CreateRouteEndpoint(string method, string route, string handlerDisplayName, Type producesType)
    {
        var metadata = new EndpointMetadataCollection(
            new HttpMethodMetadata(new[] { method }),
            new ProducesResponseTypeMetadata(200, producesType, ["application/json"]));

        var routePattern = RoutePatternFactory.Parse(route);
        var pipeline = new RequestDelegate([ExcludeFromCodeCoverage] (context) => Task.CompletedTask);

        return new RouteEndpoint(pipeline, routePattern, 0, metadata, handlerDisplayName);
    }

    public static RouteEndpoint CreateRouteEndpoint(string method, string route, string handlerDisplayName, string[] tags)
    {
        var metadata = new EndpointMetadataCollection(
            new HttpMethodMetadata(new[] { method }),
            new TagsAttribute(tags));

        var routePattern = RoutePatternFactory.Parse(route);
        var pipeline = new RequestDelegate([ExcludeFromCodeCoverage] (context) => Task.CompletedTask);

        return new RouteEndpoint(pipeline, routePattern, 0, metadata, handlerDisplayName);
    }
}
