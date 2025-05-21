using D20Tek.MinimalApi.DevView.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace D20Tek.MinimalApi.DevView;

public static class DependencyInjection
{
    public static IApplicationBuilder UseDevView(
        this IApplicationBuilder app,
        Action<DevViewOptions>? configureOptions = null)
    {
        var options = new DevViewOptions();
        configureOptions?.Invoke(options);

        if (app.ApplicationServices.GetRequiredService<IHostEnvironment>().IsProduction())
            return app;

        app.UseMiddleware<RequestLoggingMiddleware>();

        return app.UseEndpoints(endpoints =>
        {
            endpoints.MapInfoEndpoint(options);
            endpoints.MapRoutesExplorer(options);
        });
    }
}
