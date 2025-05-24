using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace D20Tek.MinimalApi.DevView.Endpoints.Dependencies;

public static partial class DependenciesEndpoint
{
    public static IEndpointRouteBuilder MapDependenciesExplorer(this IEndpointRouteBuilder endpoints, DevViewOptions options)
    {
        var basePath = options.BasePath;
        endpoints.MapGet($"{basePath}/deps", GetDependencyInfo);

        return endpoints;
    }

    internal static IResult GetDependencyInfo(HttpContext context)
    {
        var services = context.RequestServices.GetService<IRegisteredServicesProvider>();
        ArgumentNullException.ThrowIfNull(services, nameof(IRegisteredServicesProvider));

        var deps = services.Services.Where(sd => sd.ServiceType is not null)
                                    .Select(sd => CreateDependencyInfo(sd))
                                    .ToArray();

        return Results.Json(deps);
    }

    private static DependencyInfo CreateDependencyInfo(ServiceDescriptor descriptor)
    {
        var implementationType = GetImplementationType(descriptor);
        return new(
            descriptor.ServiceType.Name!,
            implementationType.FullName,
            descriptor.Lifetime.ToString(),
            implementationType.Assembly.GetName().Name);
    }

    internal static Type GetImplementationType(ServiceDescriptor descriptor) =>
        descriptor switch
        {
            { ImplementationType: not null } => descriptor.ImplementationType,
            { ImplementationInstance: not null } => descriptor.ImplementationInstance.GetType(),
            { ImplementationFactory: not null } => descriptor.ImplementationFactory.GetType(),
            { KeyedImplementationType: not null } => descriptor.KeyedImplementationType,
            { KeyedImplementationInstance: not null } => descriptor.KeyedImplementationInstance.GetType(),
            { KeyedImplementationFactory: not null } => descriptor.KeyedImplementationFactory.GetType(),
            _ => throw new ArgumentNullException(nameof(descriptor)),
        };
}
