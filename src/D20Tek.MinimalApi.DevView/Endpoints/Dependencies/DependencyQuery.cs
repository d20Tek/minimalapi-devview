using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace D20Tek.MinimalApi.DevView.Endpoints.Dependencies;

public class DependencyQuery
{
    [FromQuery]
    public string? Implementation { get; init; }

    [FromQuery]
    public string? Lifetime { get; init; }

    [FromQuery]
    public string? ServiceType { get; init; }

    [FromQuery]
    public string? Assembly { get; init; }

    public static DependencyQuery Create(IQueryCollection queryString)
    {
        ArgumentNullException.ThrowIfNull(queryString, nameof(queryString));
        return new()
        {
            Implementation = queryString["implementation"],
            Lifetime = queryString["lifetime"],
            ServiceType = queryString["serviceType"],
            Assembly = queryString["assembly"]
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