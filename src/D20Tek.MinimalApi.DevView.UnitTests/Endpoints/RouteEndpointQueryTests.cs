using D20Tek.MinimalApi.DevView.Endpoints.Routes;

namespace D20Tek.MinimalApi.DevView.UnitTests.Endpoints;

[TestClass]
public class RouteEndpointQueryTests
{
    [TestMethod]
    public void ApplyFilters_WithRoute_ReturnsFilteredList()
    {
        // arrange
        var metadata = TestRouteEndpointFactory.CreateWithRoute();
        var query = new RouteEndpointQuery { Route = "v2" };

        // act
        var result = query.ApplyFilters(metadata);

        // assert
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Count());
        Assert.AreEqual("/v2/test-endpoint", result.First()["Pattern"]);
    }

    [TestMethod]
    public void ApplyFilters_WithFakeRoute_ReturnsEmptyList()
    {
        // arrange
        var metadata = TestRouteEndpointFactory.CreateWithRoute();
        var query = new RouteEndpointQuery { Route = "bogus" };

        // act
        var result = query.ApplyFilters(metadata);

        // assert
        Assert.IsNotNull(result);
        Assert.AreEqual(0, result.Count());
    }

    [TestMethod]
    public void ApplyFilters_WithMethod_ReturnsFilteredList()
    {
        // arrange
        var metadata = TestRouteEndpointFactory.CreateWithMethod();
        var query = new RouteEndpointQuery { Method = "GET" };

        // act
        var result = query.ApplyFilters(metadata);

        // assert
        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Count());
        CollectionAssert.AreEqual(new string[] { "GET" }, (string[])result.First()["Method"]!);
    }

    [TestMethod]
    public void ApplyFilters_WithFakeMethod_ReturnsEmptyList()
    {
        // arrange
        var metadata = TestRouteEndpointFactory.CreateWithMethod();
        var query = new RouteEndpointQuery { Method = "delete" };

        // act
        var result = query.ApplyFilters(metadata);

        // assert
        Assert.IsNotNull(result);
        Assert.AreEqual(0, result.Count());
    }

    [TestMethod]
    public void ApplyFilters_WithName_ReturnsFilteredList()
    {
        // arrange
        var metadata = TestRouteEndpointFactory.CreateWithName();
        var query = new RouteEndpointQuery { EndpointName = "Endpoint" };

        // act
        var result = query.ApplyFilters(metadata);

        // assert
        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Count());
        Assert.AreEqual("Endpoint1", result.First()["Name"]);
    }

    [TestMethod]
    public void ApplyFilters_WithFakeName_ReturnsEmptyList()
    {
        // arrange
        var metadata = TestRouteEndpointFactory.CreateWithName();
        var query = new RouteEndpointQuery { EndpointName = "foo" };

        // act
        var result = query.ApplyFilters(metadata);

        // assert
        Assert.IsNotNull(result);
        Assert.AreEqual(0, result.Count());
    }

    [TestMethod]
    public void ApplyFilters_WithTag_ReturnsFilteredList()
    {
        // arrange
        var metadata = TestRouteEndpointFactory.CreateWithTags();
        var query = new RouteEndpointQuery { Tag = "foo" };

        // act
        var result = query.ApplyFilters(metadata);

        // assert
        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Count());
        CollectionAssert.AreEqual(new string[] { "foo" }, (string[])result.First()["Tags"]!);
    }

    [TestMethod]
    public void ApplyFilters_WithFakeTag_ReturnsEmptyList()
    {
        // arrange
        var metadata = TestRouteEndpointFactory.CreateWithTags();
        var query = new RouteEndpointQuery { Tag = "none" };

        // act
        var result = query.ApplyFilters(metadata);

        // assert
        Assert.IsNotNull(result);
        Assert.AreEqual(0, result.Count());
    }

    [TestMethod]
    public void ApplyFilters_WithMissingStringResult_ReturnsEmptyList()
    {
        // arrange
        var metadata = TestRouteEndpointFactory.CreateWithNullName();
        var query = new RouteEndpointQuery { EndpointName = "none" };

        // act
        var result = query.ApplyFilters(metadata);

        // assert
        Assert.IsNotNull(result);
        Assert.AreEqual(0, result.Count());
    }

    [TestMethod]
    public void ApplyFilters_WithMissingStringArrayResult_ReturnsEmptyList()
    {
        // arrange
        var metadata = TestRouteEndpointFactory.CreateWithNullTags();
        var query = new RouteEndpointQuery { Tag = "none" };

        // act
        var result = query.ApplyFilters(metadata);

        // assert
        Assert.IsNotNull(result);
        Assert.AreEqual(0, result.Count());
    }

    [TestMethod]
    public void ApplyFilters_WithMissingKeyOfList_ReturnsEmptyList()
    {
        // arrange
        var metadata = TestRouteEndpointFactory.CreateWithName();
        var query = new RouteEndpointQuery { Tag = "foo" };

        // act
        var result = query.ApplyFilters(metadata);

        // assert
        Assert.IsNotNull(result);
        Assert.AreEqual(0, result.Count());
    }

    [TestMethod]
    public void ApplyFilters_WithMissingKeyOfString_ReturnsEmptyList()
    {
        // arrange
        var metadata = TestRouteEndpointFactory.CreateWithName();
        var query = new RouteEndpointQuery { Route = "foo" };

        // act
        var result = query.ApplyFilters(metadata);

        // assert
        Assert.IsNotNull(result);
        Assert.AreEqual(0, result.Count());
    }
}
