using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace D20Tek.MinimalApi.DevView.Endpoints.Dependencies;

public sealed class DependencyQuery
{
    private const string _implementationKey = "implementation";
    private const string _lifetimeKey = "lifetime";
    private const string _serviceTypeKey = "serviceType";
    private const string _assemblyKey = "assembly";

    public string? Implementation { get; init; }

    public string? Lifetime { get; init; }

    public string? ServiceType { get; init; }

    public string? Assembly { get; init; }

    public static DependencyQuery Create(IQueryCollection query)
    {
        ArgumentNullException.ThrowIfNull(query, nameof(query));
        return new()
        {
            Implementation = query[_implementationKey],
            Lifetime = query[_lifetimeKey],
            ServiceType = query[_serviceTypeKey],
            Assembly = query[_assemblyKey]
        };
    }

    public IEnumerable<ServiceDescriptor> ApplyFilters(IEnumerable<ServiceDescriptor> descriptors)
    {
        var filtered = descriptors;
        if (string.IsNullOrWhiteSpace(Implementation) is false)
        {
            filtered = filtered.Where(
                sd => sd.GetCompositeImplementationType()
                        .FullName!
                        .Contains(Implementation, StringComparison.OrdinalIgnoreCase) is true);
        }

        if (Enum.TryParse<ServiceLifetime>(Lifetime, true, out var lifetime))
        {
            filtered = filtered.Where(sd => sd.Lifetime == lifetime);
        }

        if (string.IsNullOrWhiteSpace(ServiceType) is false)
        {
            filtered = filtered.Where(sd => sd.ServiceType.FullName!
                                              .Contains(ServiceType, StringComparison.OrdinalIgnoreCase) is true);
        }

        if (string.IsNullOrWhiteSpace(Assembly) is false)
        {
            filtered = filtered.Where(
                sd => sd.GetCompositeImplementationType()
                        .Assembly.GetName().Name!
                        .Contains(Assembly, StringComparison.OrdinalIgnoreCase) is true);
        }

        return filtered;
    }
}