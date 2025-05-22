using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        endpoints.MapGet($"{basePath}/info", GetDevInfo);
        return endpoints;
    }

    internal static IResult GetDevInfo([FromServices]IHostEnvironment env)
    {
        var assembly = Assembly.GetEntryAssembly();
        var version = assembly!.GetName().Version!.ToString();
        var startTime = Process.GetCurrentProcess().StartTime.ToUniversalTime();

        return Results.Json(new InfoResponse(
            env.ApplicationName,
            env.EnvironmentName,
            version,
            startTime,
            (int)(DateTime.UtcNow - startTime).TotalSeconds));
    }

    internal class InfoResponse
    {
        public string AppName { get; }
        public string Environment {  get; }
        public string Version { get; }
        public DateTime StartTime { get; }
        public int UptimeSeconds { get; }

        public InfoResponse(string appName, string environment, string version, DateTime startTime, int uptimeSeconds)
        {
            AppName = appName;
            Environment = environment;
            Version = version;
            StartTime = startTime;
            UptimeSeconds = uptimeSeconds;
        }
    }
}
