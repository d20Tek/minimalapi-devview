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

    internal static IResult GetConfigInfo(IConfiguration config, HttpContext context, IOptions<DevViewOptions> options)
    {
        ArgumentNullException.ThrowIfNull(config, nameof(config));

        var query = ConfigQuery.Create(context.Request.Query);
        var configs = GetDetailedConfigEntries(config, query, options.Value);
        return Results.Json(configs);
    }

    private static IList<ConfigInfo> GetDetailedConfigEntries(
        IConfiguration config,
        ConfigQuery query,
        DevViewOptions options)
    {
        if (config is not IConfigurationRoot root) return [];

        var keys = config.AsEnumerable().Select(kvp => kvp.Key).Distinct().ToList();
        var configs = root.GetConfigDetails(keys, options);
        return query.ApplyFilters(configs).ToList();
    }
}
