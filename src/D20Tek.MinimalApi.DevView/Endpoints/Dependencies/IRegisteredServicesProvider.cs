namespace D20Tek.MinimalApi.DevView.Endpoints.Dependencies;

public interface IRegisteredServicesProvider
{
    IReadOnlyList<ServiceDescriptor> Services { get; }
}
