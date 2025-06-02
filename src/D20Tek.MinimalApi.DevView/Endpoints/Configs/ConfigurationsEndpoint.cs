using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;

namespace D20Tek.MinimalApi.DevView.Endpoints.Configs;

public static class ConfigurationsEndpoint
{
    public static IEndpointRouteBuilder MapConfigurationExplorer(
        this IEndpointRouteBuilder endpoints,
        DevViewOptions options)
    {
        var basePath = options.BasePath;
        endpoints.MapGet($"{basePath}/config", GetConfigInfo).WithTags("DevView");

        return endpoints;
    }

    internal static IResult GetConfigInfo(IConfiguration config)
    {
        ArgumentNullException.ThrowIfNull(config, nameof(config));

        var configs = Enumerable.Empty<ConfigInfo>();
        return Results.Json(configs);
    }
}
