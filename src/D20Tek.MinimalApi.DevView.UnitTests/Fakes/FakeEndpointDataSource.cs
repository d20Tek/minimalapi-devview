using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System.Diagnostics.CodeAnalysis;

namespace D20Tek.MinimalApi.DevView.UnitTests.Fakes;

internal class FakeEndpointDataSource : EndpointDataSource
{
    private readonly IReadOnlyList<Endpoint> _endpoints;

    public FakeEndpointDataSource(IEnumerable<Endpoint> endpoints)
    {
        _endpoints = endpoints.ToList();
    }

    public override IReadOnlyList<Endpoint> Endpoints => _endpoints;

    [ExcludeFromCodeCoverage]
    public override IChangeToken GetChangeToken() => NullChangeToken.Singleton;
}
