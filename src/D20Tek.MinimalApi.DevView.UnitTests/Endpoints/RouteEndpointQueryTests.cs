using D20Tek.MinimalApi.DevView.Endpoints.Routes;

namespace D20Tek.MinimalApi.DevView.UnitTests.Endpoints;

[TestClass]
public class RouteEndpointQueryTests
{
    [TestMethod]
    public void ApplyFilters_WithRoute_ReturnsFilteredList()
    {
        // arrange
        var metadata = new List<Dictionary<string, object?>>()
        {
            new Dictionary<string, object?>()
            {
                { "Pattern", "/v1/test-endpoint" }
            },
            new Dictionary<string, object?>()
            {
                { "Pattern", "/v2/test-endpoint" }
            },
            new Dictionary<string, object?>()
            {
                { "Pattern", "/v3/test-endpoint" }
            }
        };

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
        var metadata = new List<Dictionary<string, object?>>()
        {
            new Dictionary<string, object?>()
            {
                { "Pattern", "/v1/test-endpoint" }
            },
            new Dictionary<string, object?>()
            {
                { "Pattern", "/v2/test-endpoint" }
            },
            new Dictionary<string, object?>()
            {
                { "Pattern", "/v3/test-endpoint" }
            }
        };

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
        var metadata = new List<Dictionary<string, object?>>()
        {
            new Dictionary<string, object?>()
            {
                { "Method", new string[] { "GET" } }
            },
            new Dictionary<string, object?>()
            {
                { "Method", new string[] { "POST" } }
            },
            new Dictionary<string, object?>()
            {
                { "Method", new string[] { "GET" } }
            }
        };

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
        var metadata = new List<Dictionary<string, object?>>()
        {
            new Dictionary<string, object?>()
            {
                { "Method", new string[] { "GET" } }
            },
            new Dictionary<string, object?>()
            {
                { "Method", new string[] { "POST" } }
            },
            new Dictionary<string, object?>()
            {
                { "Method", new string[] { "GET" } }
            }
        };

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
        var metadata = new List<Dictionary<string, object?>>()
        {
            new Dictionary<string, object?>()
            {
                { "Name", "Endpoint1" }
            },
            new Dictionary<string, object?>()
            {
                { "Name", "Func2" }
            },
            new Dictionary<string, object?>()
            {
                { "Name", "Endpoint3" }
            }
        };

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
        var metadata = new List<Dictionary<string, object?>>()
        {
            new Dictionary<string, object?>()
            {
                { "Name", "Endpoint1" }
            },
            new Dictionary<string, object?>()
            {
                { "Name", "Func2" }
            },
            new Dictionary<string, object?>()
            {
                { "Name", "Endpoint3" }
            }
        };

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
        var metadata = new List<Dictionary<string, object?>>()
        {
            new Dictionary<string, object?>()
            {
                { "Tags", new string[] { "foo" } }
            },
            new Dictionary<string, object?>()
            {
                { "Tags", new string[] { "foo", "bar" } }
            },
            new Dictionary<string, object?>()
            {
                { "Tags", new string[] { "baz" } }
            }
        };

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
        var metadata = new List<Dictionary<string, object?>>()
        {
            new Dictionary<string, object?>()
            {
                { "Tags", new string[] { "foo" } }
            },
            new Dictionary<string, object?>()
            {
                { "Tags", new string[] { "foo", "bar" } }
            },
            new Dictionary<string, object?>()
            {
                { "Tags", new string[] { "baz" } }
            }
        };

        var query = new RouteEndpointQuery { Tag = "none" };

        // act
        var result = query.ApplyFilters(metadata);

        // assert
        Assert.IsNotNull(result);
        Assert.AreEqual(0, result.Count());
    }
}
