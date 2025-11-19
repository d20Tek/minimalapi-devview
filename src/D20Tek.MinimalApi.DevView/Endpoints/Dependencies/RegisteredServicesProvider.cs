using Microsoft.Extensions.DependencyInjection;

namespace D20Tek.MinimalApi.DevView.Endpoints.Dependencies;

internal class RegisteredServicesProvider(IEnumerable<ServiceDescriptor> services) : IRegisteredServicesProvider
{
    public IReadOnlyList<ServiceDescriptor> Services { get; } = services.ToList().AsReadOnly();
}