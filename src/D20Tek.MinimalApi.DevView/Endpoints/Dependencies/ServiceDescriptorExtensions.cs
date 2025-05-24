using Microsoft.Extensions.DependencyInjection;

namespace D20Tek.MinimalApi.DevView.Endpoints.Dependencies;

internal static class ServiceDescriptorExtensions
{
    public static Type GetCompositeImplementationType(this ServiceDescriptor descriptor) =>
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
