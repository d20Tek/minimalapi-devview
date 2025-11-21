namespace D20Tek.MinimalApi.DevView.Endpoints.Routes;

internal static partial class RouteEndpointExtensions
{
    private const string _noData = "none";
    private const string _methodKey = "Method";
    private const string _patternKey = "Pattern";
    private const string _tagsKey = "Tags";
    private const string _handlerKey = "Handler";
    private const string _nameKey = "Name";
    private const string _producesKey = "Produces";
    private const string _metadataTypesKey = "MetadataTypes";
    private static string FormatMetadataType(string name, object value) => $"Type: {name}, Value: {value}";

    public static Dictionary<string, object?> InspectEndpoint(this RouteEndpoint endpoint, DevViewOptions options)
    {
        var routePattern = endpoint.GetRoutePattern();
        if (routePattern.StartsWith(options.BasePath)) return [];

        var routeInfo = new Dictionary<string, object?>
        {
            [_methodKey] = endpoint.GetHttpMethods(),
            [_patternKey] = routePattern
        };

        if (options.IncludeRouteMetadata)
        {
            routeInfo[_tagsKey] = endpoint.GetTags();
            routeInfo[_handlerKey] = endpoint.RequestDelegate!.Method.Name;
            routeInfo[_nameKey] = endpoint.GetEndpointName();

            var produces = endpoint.GetProducesResponses();
            if (produces.Length > 0)
            {
                routeInfo[_producesKey] = produces;
            }
        }

        if (options.IncludeRouteDebugDetails)
        {
            routeInfo[_metadataTypesKey] = endpoint.Metadata.Select(x => FormatMetadataType(x.GetType().Name, x))
                                                            .ToArray();
        }

        return routeInfo;
    }
}
