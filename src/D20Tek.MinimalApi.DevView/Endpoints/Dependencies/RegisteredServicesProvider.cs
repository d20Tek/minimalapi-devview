namespace D20Tek.MinimalApi.DevView.Endpoints.Dependencies;

internal sealed class RegisteredServicesProvider(IEnumerable<ServiceDescriptor> services)
    : IRegisteredServicesProvider
{
    public IReadOnlyList<ServiceDescriptor> Services { get; } = services.ToList().AsReadOnly();
}