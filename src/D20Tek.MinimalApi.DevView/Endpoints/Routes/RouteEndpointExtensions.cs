using Microsoft.AspNetCore.Routing;

namespace D20Tek.MinimalApi.DevView.Endpoints.Routes;

internal static partial class RouteEndpointExtensions
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

            var produces = endpoint.GetProducesResponses();
            if (produces.Length > 0)
            {
                routeInfo["Produces"] = produces;
            }
        }

        if (options.IncludeRouteDebugDetails)
        {
            routeInfo["MetadataTypes"] = endpoint.Metadata.Select(GetTypeMetadata)
                                                          .ToArray();
        }

        return routeInfo;
    }

    private static string GetTypeMetadata(object t) => $"Type: {t.GetType().Name}, Value:{t}";
}
