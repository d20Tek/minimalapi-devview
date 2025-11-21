using D20Tek.MinimalApi.DevView.Endpoints.Configs;
using D20Tek.MinimalApi.DevView.Endpoints.Dependencies;
using D20Tek.MinimalApi.DevView.Endpoints.Info;
using D20Tek.MinimalApi.DevView.Endpoints.Routes;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace D20Tek.MinimalApi.DevView;

public static class DependencyInjection
{
    private const string _devViewSection = "DevView";

    public static IServiceCollection AddDevView(this IServiceCollection services, IConfiguration config) =>
        services.Configure<DevViewOptions>(config.GetSection(_devViewSection))
                .AddSingleton<IRegisteredServicesProvider>(sp => new RegisteredServicesProvider(services));

    public static IApplicationBuilder UseDevView(
        this IApplicationBuilder app,
        Action<DevViewOptions>? configureOptions = null)
    {
        if (app.ApplicationServices.GetRequiredService<IHostEnvironment>().IsProduction())
            return app;

        var options = app.ApplicationServices.GetRequiredService<IOptions<DevViewOptions>>();
        configureOptions?.Invoke(options.Value);

        app.UseMiddleware<RequestLoggingMiddleware>();
        app.UseRouting();

        return app.UseEndpoints(endpoints =>
        {
            endpoints.MapInfoEndpoint(options.Value);
            endpoints.MapRoutesExplorer(options.Value);
            endpoints.MapDependenciesExplorer(options.Value);
            endpoints.MapConfigurationExplorer(options.Value);
        });
    }
}
