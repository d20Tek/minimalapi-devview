using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace D20Tek.MinimalApi.DevView.Endpoints.Routes;

internal static partial class RouteEndpointExtensions
{
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
                                 Key = string.Join(",", meta.ContentTypes.OrderBy(c => c))
                             })
                             // Prefer the more specific ResponseType (i.e., not IResult)
                             .Select(g => g.FirstOrDefault(x => x.Type != typeof(IResult)) ?? g.First())
                             .Select(FormatProducesMetadata)];

    private static string FormatProducesMetadata(ProducesResponseTypeMetadata meta) =>
        $"StatusCode: {meta.StatusCode}, ContentTypes: {string.Join(";", meta.ContentTypes)}, Type: {meta.Type}";
}
