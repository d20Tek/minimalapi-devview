using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using System.Reflection;

namespace D20Tek.MinimalApi.DevView.Endpoints;

public static class InfoEndpoint
{
    public static IEndpointRouteBuilder MapInfoEndpoint(this IEndpointRouteBuilder endpoints, DevViewOptions options)
    {
        var basePath = options.BasePath;
        endpoints.MapGet($"{basePath}/info", (IHostEnvironment env) =>
        {
            var assembly = Assembly.GetEntryAssembly();
            var version = assembly?.GetName().Version?.ToString() ?? "unknown";
            var startTime = Process.GetCurrentProcess().StartTime.ToUniversalTime();

            return Results.Json(new
            {
                AppName = env.ApplicationName,
                Environment = env.EnvironmentName,
                Version = version,
                StartTime = startTime,
                UptimeSeconds = (int)(DateTime.UtcNow - startTime).TotalSeconds
            });
        });

        return endpoints;
    }
}
