using Microsoft.Extensions.DependencyInjection;

namespace D20Tek.MinimalApi.DevView.Services;

internal class RegisteredServicesProvider : IRegisteredServicesProvider
{
    public IReadOnlyList<ServiceDescriptor> Services { get; }

    public RegisteredServicesProvider(IEnumerable<ServiceDescriptor> services)
    {
        Services = services.ToList().AsReadOnly();
    }
}