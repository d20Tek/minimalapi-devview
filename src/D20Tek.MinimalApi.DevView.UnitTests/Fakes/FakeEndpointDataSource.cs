using Microsoft.Extensions.FileProviders;

namespace D20Tek.MinimalApi.DevView.UnitTests.Fakes;

internal class FakeEndpointDataSource(IEnumerable<Endpoint> endpoints) : EndpointDataSource
{
    private readonly IReadOnlyList<Endpoint> _endpoints = [.. endpoints];

    public override IReadOnlyList<Endpoint> Endpoints => _endpoints;

    [ExcludeFromCodeCoverage]
    public override IChangeToken GetChangeToken() => NullChangeToken.Singleton;
}
