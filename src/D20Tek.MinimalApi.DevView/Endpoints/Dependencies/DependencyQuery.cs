using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace D20Tek.MinimalApi.DevView.Endpoints.Dependencies;

public class DependencyQuery
{
    public string? Implementation { get; init; }

    public string? Lifetime { get; init; }

    public string? ServiceType { get; init; }

    public string? Assembly { get; init; }

    public static DependencyQuery Create(IQueryCollection query)
    {
        ArgumentNullException.ThrowIfNull(query, nameof(query));
        return new()
        {
            Implementation = query["implementation"],
            Lifetime = query["lifetime"],
            ServiceType = query["serviceType"],
            Assembly = query["assembly"]
        };
    }

    public IEnumerable<ServiceDescriptor> ApplyFilters(IEnumerable<ServiceDescriptor> descriptors)
    {
        var filtered = descriptors;
        if (string.IsNullOrWhiteSpace(Implementation) is false)
        {
            filtered = filtered.Where(
                sd => sd.GetCompositeImplementationType().FullName!.Contains(Implementation) == true);
        }

        if (Enum.TryParse<ServiceLifetime>(Lifetime, true, out var lifetime))
        {
            filtered = filtered.Where(sd => sd.Lifetime == lifetime);
        }

        if (string.IsNullOrWhiteSpace(ServiceType) is false)
        {
            filtered = filtered.Where(sd => sd.ServiceType.FullName!.Contains(ServiceType) == true);
        }

        if (string.IsNullOrWhiteSpace(Assembly) is false)
        {
            filtered = filtered.Where(
                sd => sd.GetCompositeImplementationType().Assembly.GetName().Name!.Contains(Assembly) == true);
        }

        return filtered;
    }
}