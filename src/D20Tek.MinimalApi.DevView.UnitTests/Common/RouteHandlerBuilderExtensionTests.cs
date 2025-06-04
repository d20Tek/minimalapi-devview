using D20Tek.MinimalApi.DevView.Common;
using Microsoft.AspNetCore.Builder;

namespace D20Tek.MinimalApi.DevView.UnitTests.Common;

[TestClass]
public class RouteHandlerBuilderExtensionTests
{
    [TestMethod]
    public void WithDevEndpointVisibility_HideDevEndpoints_AddsMetadata()
    {
        // arrange
        var routeBuilder = new FakeRouteBuilder();

        // act
        var result = routeBuilder.WithDevEndpointVisibility(true);

        // assert
        Assert.IsNotNull(result);
        Assert.IsTrue(routeBuilder.HasExcludeMetadata);
    }

    [TestMethod]
    public void WithDevEndpointVisibility_ShowDevEndpoints_DoesNotAddMetadata()
    {
        // arrange
        var routeBuilder = new FakeRouteBuilder();

        // act
        var result = routeBuilder.WithDevEndpointVisibility(false);

        // assert
        Assert.IsNotNull(result);
        Assert.IsFalse(routeBuilder.HasExcludeMetadata);
    }

    private class FakeRouteBuilder : IEndpointConventionBuilder
    {
        public bool HasExcludeMetadata { get; private set; }

        public void Add(Action<EndpointBuilder> convention)
        {
            ArgumentNullException.ThrowIfNull(convention, nameof(convention));
            HasExcludeMetadata = true;
        }
    }
}
