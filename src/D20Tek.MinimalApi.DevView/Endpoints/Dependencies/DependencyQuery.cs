using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace D20Tek.MinimalApi.DevView.Endpoints.Dependencies;

public class DependencyQuery
{
    [FromQuery]
    public string? Namespace { get; init; }

    [FromQuery]
    public string? Lifetime { get; init; }

    [FromQuery]
    public string? ServiceContains { get; init; }

    [FromQuery]
    public string? Assembly { get; init; }

    public static DependencyQuery Create(IQueryCollection queryString)
    {
        ArgumentNullException.ThrowIfNull(queryString, nameof(queryString));
        return new()
        {
            Namespace = queryString["namespace"],
            Lifetime = queryString["lifetime"],
            ServiceContains = queryString["serviceContains"],
            Assembly = queryString["assembly"]
        };
    }

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