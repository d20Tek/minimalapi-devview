using D20Tek.MinimalApi.DevView.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace D20Tek.MinimalApi.DevView.Endpoints.Dependencies;

public static partial class DependenciesEndpoint
{
    public static IEndpointRouteBuilder MapDependenciesExplorer(
        this IEndpointRouteBuilder endpoints,
        DevViewOptions options)
    {
        endpoints.MapGet(Constants.Dependencies.EndpointPattern(options.BasePath), GetDependencyInfo)
                 .WithTags(Constants.EndpointTags)
                 .WithName(Constants.Dependencies.EndpointName)
                 .Produces<DependencyInfo[]>()
                 .WithDevEndpointVisibility(options.HideDevEndpointsFromOpenApi);

        return endpoints;
    }

    internal static IResult GetDependencyInfo(HttpContext context)
    {
        var services = context.RequestServices.GetService<IRegisteredServicesProvider>();
        ArgumentNullException.ThrowIfNull(services, nameof(IRegisteredServicesProvider));

        var query = DependencyQuery.Create(context.Request.Query);
        var deps = services.Services.Where(sd => sd.ServiceType is not null)
                                    .Filter(query)
                                    .Select(sd => CreateDependencyInfo(sd, sd.GetCompositeImplementationType()))
                                    .ToArray();

        return Results.Json(deps);
    }

    private static DependencyInfo CreateDependencyInfo(ServiceDescriptor descriptor, Type implementationType) =>
        new(
            descriptor.ServiceType.FullName!,
            implementationType.FullName,
            descriptor.Lifetime.ToString(),
            implementationType.Assembly.GetName().Name);

    private static IEnumerable<ServiceDescriptor> Filter(
        this IEnumerable<ServiceDescriptor> descriptors,
        DependencyQuery query) =>
        query.ApplyFilters(descriptors);
}
