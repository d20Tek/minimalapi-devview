namespace D20Tek.MinimalApi.DevView.Endpoints.Routes;

internal static partial class RouteEndpointExtensions
{
    private const string _contentTypesSeparator = ",";
    private const string _formatSeparator = ";";
    private static string MetadataText(int statusCode, string contentTypes, Type? type) =>
        $"StatusCode: {statusCode}, ContentTypes: {contentTypes}, Type: {type}";

    private static string GetRoutePattern(this RouteEndpoint endpoint)
    {
        var pattern = endpoint.RoutePattern.RawText;
        return string.IsNullOrEmpty(pattern) ? _noData : pattern;
    }

    private static string[] GetHttpMethods(this RouteEndpoint endpoint) =>
        [.. endpoint.Metadata.OfType<HttpMethodMetadata>()
                             .SelectMany(m => m.HttpMethods)
                             .DefaultIfEmpty(_noData)];

    private static string[] GetTags(this RouteEndpoint endpoint) =>
        [.. endpoint.Metadata.OfType<TagsAttribute>().SelectMany(m => m.Tags)];

    private static string? GetEndpointName(this RouteEndpoint endpoint) =>
        endpoint.Metadata.OfType<EndpointNameMetadata>()
                         .FirstOrDefault()?
                         .EndpointName;

    private static string[] GetProducesResponses(this RouteEndpoint endpoint) =>
        [.. endpoint.Metadata.OfType<ProducesResponseTypeMetadata>()
                             .GroupBy(meta => new
                             {
                                 meta.StatusCode,
                                 Key = string.Join(_contentTypesSeparator, meta.ContentTypes.OrderBy(c => c))
                             })
                             // Prefer the more specific ResponseType (i.e., not IResult)
                             .Select(g => g.FirstOrDefault(x => x.Type != typeof(IResult)) ?? g.First())
                             .Select(FormatProducesMetadata)];

    private static string FormatProducesMetadata(ProducesResponseTypeMetadata meta) =>
        MetadataText(meta.StatusCode, string.Join(_formatSeparator, meta.ContentTypes), meta.Type);
}
