using D20Tek.MinimalApi.DevView.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace D20Tek.MinimalApi.DevView;

public static class DependencyInjection
{
    public static IServiceCollection AddDevView(this IServiceCollection services, IConfiguration config) =>
        services.Configure<DevViewOptions>(config.GetSection("DevView"));

    public static IApplicationBuilder UseDevView(
        this IApplicationBuilder app,
        Action<DevViewOptions>? configureOptions = null)
    {
        var options = app.ApplicationServices.GetRequiredService<IOptions<DevViewOptions>>();
        configureOptions?.Invoke(options.Value);

        if (app.ApplicationServices.GetRequiredService<IHostEnvironment>().IsProduction())
            return app;

        app.UseMiddleware<RequestLoggingMiddleware>();
        app.UseRouting();

        return app.UseEndpoints(endpoints =>
        {
            endpoints.MapInfoEndpoint(options.Value);
            endpoints.MapRoutesExplorer(options.Value);
        });
    }
}
