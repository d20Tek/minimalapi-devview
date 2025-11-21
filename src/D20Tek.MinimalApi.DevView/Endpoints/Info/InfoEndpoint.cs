using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using System.Reflection;

namespace D20Tek.MinimalApi.DevView.Endpoints.Info;

public static partial class InfoEndpoint
{
    public static IEndpointRouteBuilder MapInfoEndpoint(this IEndpointRouteBuilder endpoints, DevViewOptions options)
    {
        endpoints.MapGet(Constants.Info.EndpointPattern(options.BasePath), GetDevInfo)
                 .WithTags(Constants.EndpointTags)
                 .WithName(Constants.Info.EndpointName)
                 .Produces<InfoResponse>()
                 .WithDevEndpointVisibility(options.HideDevEndpointsFromOpenApi);

        return endpoints;
    }

    internal static IResult GetDevInfo([FromServices] IHostEnvironment env) =>
        Results.Json(CreateResponse(
            env,
            Assembly.GetEntryAssembly()!.GetName().Version!.ToString(),
            Process.GetCurrentProcess().StartTime.ToUniversalTime()));

    private static InfoResponse CreateResponse(IHostEnvironment env, string version, DateTime startTime) =>
        new(
            env.ApplicationName,
            env.EnvironmentName,
            version,
            startTime,
            (int)(DateTime.UtcNow - startTime).TotalSeconds);
}
