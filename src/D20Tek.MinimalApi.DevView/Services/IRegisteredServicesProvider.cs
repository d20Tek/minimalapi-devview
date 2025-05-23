using Microsoft.Extensions.DependencyInjection;

namespace D20Tek.MinimalApi.DevView.Services;

public interface IRegisteredServicesProvider
{
    IReadOnlyList<ServiceDescriptor> Services { get; }
}
