using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

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

    internal static IResult GetConfigInfo(IConfiguration config, IOptions<DevViewOptions> options)
    {
        ArgumentNullException.ThrowIfNull(config, nameof(config));

        var configs = GetDetailedConfigEntries(config, options.Value);
        return Results.Json(configs);
    }

    private static IList<ConfigInfo> GetDetailedConfigEntries(IConfiguration config, DevViewOptions options)
    {
        if (config is not IConfigurationRoot root) return [];

        var keys = config.AsEnumerable().Select(kvp => kvp.Key).Distinct().ToList();
        var configs = root.GetConfigDetails(keys, options);
        return configs;
    }
}
