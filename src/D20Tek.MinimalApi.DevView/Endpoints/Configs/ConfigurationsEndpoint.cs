using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using D20Tek.MinimalApi.DevView.Common;

namespace D20Tek.MinimalApi.DevView.Endpoints.Configs;

public static class ConfigurationsEndpoint
{
    public static IEndpointRouteBuilder MapConfigurationExplorer(
        this IEndpointRouteBuilder endpoints,
        DevViewOptions options)
    {
        var basePath = options.BasePath;
        endpoints.MapGet($"{basePath}/config", GetConfigInfo)
                 .WithTags("DevView")
                 .WithName("GetDevConfiguration")
                 .Produces<ConfigResponse>()
                 .WithDevEndpointVisibility(options.HideDevEndpointsFromOpenApi);

        return endpoints;
    }

    internal static IResult GetConfigInfo(
        IConfiguration config,
        IWebHostEnvironment hostEnv,
        HttpContext context,
        IOptions<DevViewOptions> options)
    {
        ArgumentNullException.ThrowIfNull(config, nameof(config));

        var query = ConfigQuery.Create(context.Request.Query);
        var configs = GetDetailedConfigInfo(config, hostEnv, query, options.Value);
        return Results.Json(configs);
    }

    private static ConfigResponse? GetDetailedConfigInfo(
        IConfiguration config,
        IWebHostEnvironment hostEnv,
        ConfigQuery query,
        DevViewOptions options)
    {
        if (config is not IConfigurationRoot root) return null;

        var keys = config.AsEnumerable().Select(kvp => kvp.Key).Distinct().ToList();

        var configs = root.GetConfigDetails(keys, options);
        var filtered = query.ApplyFilters(configs).ToList();
        var summary = new SummaryBuilder(root, hostEnv, filtered).Build();

        return new(summary, filtered);
    }
}
