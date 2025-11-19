using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System.Diagnostics.CodeAnalysis;

namespace D20Tek.MinimalApi.DevView.UnitTests.Fakes;

internal class FakeEndpointDataSource(IEnumerable<Endpoint> endpoints) : EndpointDataSource
{
    private readonly IReadOnlyList<Endpoint> _endpoints = [.. endpoints];

    public override IReadOnlyList<Endpoint> Endpoints => _endpoints;

    [ExcludeFromCodeCoverage]
    public override IChangeToken GetChangeToken() => NullChangeToken.Singleton;
}
