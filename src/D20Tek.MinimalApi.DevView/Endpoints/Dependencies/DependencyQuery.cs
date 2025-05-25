using Microsoft.Extensions.DependencyInjection;

namespace D20Tek.MinimalApi.DevView.Endpoints.Dependencies;

internal class DependencyQuery
{
    public string? Namespace { get; set; }

    public string? Lifetime { get; set; }

    public string? ServiceContains { get; set; }

    public string? Assembly { get; set; }

    public IEnumerable<ServiceDescriptor> ApplyFilters(IEnumerable<ServiceDescriptor> descriptors)
    {
        var filtered = descriptors;
        if (string.IsNullOrWhiteSpace(Namespace) is false)
        {
            filtered = filtered.Where(
                sd => sd.GetCompositeImplementationType().Namespace!.StartsWith(Namespace) == true);
        }

        if (Enum.TryParse<ServiceLifetime>(Lifetime, true, out var lifetime))
        {
            filtered = filtered.Where(sd => sd.Lifetime == lifetime);
        }

        if (string.IsNullOrWhiteSpace(ServiceContains) is false)
        {
            filtered = filtered.Where(sd => sd.ServiceType.FullName!.Contains(ServiceContains) == true);
        }

        if (string.IsNullOrWhiteSpace(Assembly) is false)
        {
            filtered = filtered.Where(
                sd => sd.GetCompositeImplementationType().Assembly.GetName().Name!.Contains(Assembly) == true);
        }

        return filtered;
    }
}