using Microsoft.AspNetCore.Routing.Patterns;

namespace D20Tek.MinimalApi.DevView.UnitTests.Fakes;

internal static class EndpointDataSourceFactory
{
    public static RouteEndpoint CreateRouteEndpoint(string method, string route, string handlerDisplayName)
    {
        var metadata = new EndpointMetadataCollection(
            new HttpMethodMetadata([method]));

        var routePattern = RoutePatternFactory.Parse(route);
        var pipeline = new RequestDelegate([ExcludeFromCodeCoverage] (context) => Task.CompletedTask);

        return new RouteEndpoint(pipeline, routePattern, 0, metadata, handlerDisplayName);
    }

    public static RouteEndpoint CreateRouteEndpoint(string method, string route, string handlerDisplayName, Type producesType)
    {
        var metadata = new EndpointMetadataCollection(
            new HttpMethodMetadata([method]),
            new ProducesResponseTypeMetadata(200, producesType, ["application/json"]));

        var routePattern = RoutePatternFactory.Parse(route);
        var pipeline = new RequestDelegate([ExcludeFromCodeCoverage] (context) => Task.CompletedTask);

        return new RouteEndpoint(pipeline, routePattern, 0, metadata, handlerDisplayName);
    }

    public static RouteEndpoint CreateRouteEndpoint(string method, string route, string handlerDisplayName, Type producesType1, Type producesType2)
    {
        var metadata = new EndpointMetadataCollection(
            new HttpMethodMetadata([method]),
            new ProducesResponseTypeMetadata(200, producesType1, ["application/json"]),
            new ProducesResponseTypeMetadata(200, producesType2, ["application/json"]));

        var routePattern = RoutePatternFactory.Parse(route);
        var pipeline = new RequestDelegate([ExcludeFromCodeCoverage] (context) => Task.CompletedTask);

        return new RouteEndpoint(pipeline, routePattern, 0, metadata, handlerDisplayName);
    }

    public static RouteEndpoint CreateRouteEndpoint(string method, string route, string handlerDisplayName, string[] tags)
    {
        var metadata = new EndpointMetadataCollection(
            new HttpMethodMetadata([method]),
            new TagsAttribute(tags),
            new EndpointNameMetadata(handlerDisplayName));

        var routePattern = RoutePatternFactory.Parse(route);
        var pipeline = new RequestDelegate([ExcludeFromCodeCoverage] (context) => Task.CompletedTask);

        return new RouteEndpoint(pipeline, routePattern, 0, metadata, handlerDisplayName);
    }

    public static RouteEndpoint CreateNoPatternEndpoint(string method)
    {
        var metadata = new EndpointMetadataCollection(
            new HttpMethodMetadata(new[] { method }));

        var routePattern = RoutePatternFactory.Parse("");
        var pipeline = new RequestDelegate([ExcludeFromCodeCoverage] (context) => Task.CompletedTask);

        return new RouteEndpoint(pipeline, routePattern, 0, metadata, "Handler");
    }
}
