using D20Tek.MinimalApi.DevView.Endpoints;
using D20Tek.MinimalApi.DevView.UnitTests.Fakes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;

namespace D20Tek.MinimalApi.DevView.UnitTests.Endpoints;

[TestClass]
public sealed class RoutesEndpointTests
{
    internal class TestResponse { }

    [TestMethod]
    public void GetRoutes_WithSingleEndpoint_ReturnsExpectedJsonResult()
    {
        // arrange
        var options = Options.Create(new DevViewOptions());
        var endpoints = new List<Endpoint>
        {
            EndpointDataSourceFactory.CreateRouteEndpoint("GET", "/hello", "TestHandler", typeof(TestResponse))
        };

        var endpointSource = new FakeEndpointDataSource(endpoints);

        // act
        var result = RoutesEndpoint.GetRoutes(endpointSource, options) as IResult;

        // assert
        Assert.IsNotNull(result);
        var jsonResult = result as JsonHttpResult<IEnumerable<Dictionary<string, object>>>;
        Assert.IsNotNull(jsonResult);
        var endpointDict = jsonResult.Value!.First();
        var methods = endpointDict["Method"] as string[];
        CollectionAssert.Contains(methods, "GET");
        Assert.AreEqual("/hello", endpointDict["Pattern"]);
        Assert.AreEqual("<CreateRouteEndpoint>b__1_0", endpointDict["Handler"]);
        var produces = endpointDict["Produces"] as string[];
        Assert.AreEqual(1, produces!.Length);
        CollectionAssert.Contains(
            produces,
            "StatusCode: 200, ContentTypes: application/json, Type: D20Tek.MinimalApi.DevView.UnitTests.Endpoints.RoutesEndpointTests+TestResponse");
    }

    [TestMethod]
    public void GetRoutes_WithMultipleEndpoints_ReturnsExpectedJsonResult()
    {
        // arrange
        var options = Options.Create(new DevViewOptions());
        var endpoints = new List<Endpoint>
        {
            EndpointDataSourceFactory.CreateRouteEndpoint("GET", "/hello", "TestHandler", typeof(TestResponse)),
            EndpointDataSourceFactory.CreateRouteEndpoint("DELETE", "/hello/{id}", "DeleteHandler")
        };

        var endpointSource = new FakeEndpointDataSource(endpoints);

        // act
        var result = RoutesEndpoint.GetRoutes(endpointSource, options) as IResult;

        // assert
        Assert.IsNotNull(result);
        var jsonResult = result as JsonHttpResult<IEnumerable<Dictionary<string, object>>>;
        Assert.IsNotNull(jsonResult);
        var endpointDict = jsonResult.Value!.First();
        var methods = endpointDict["Method"] as string[];
        CollectionAssert.Contains(methods, "GET");
        Assert.AreEqual("/hello", endpointDict["Pattern"]);
        Assert.AreEqual("<CreateRouteEndpoint>b__1_0", endpointDict["Handler"]);
        var produces = endpointDict["Produces"] as string[];
        Assert.AreEqual(1, produces!.Length);
        CollectionAssert.Contains(
            produces,
            "StatusCode: 200, ContentTypes: application/json, Type: D20Tek.MinimalApi.DevView.UnitTests.Endpoints.RoutesEndpointTests+TestResponse");

        endpointDict = jsonResult.Value!.Last();
        methods = endpointDict["Method"] as string[];
        CollectionAssert.Contains(methods, "DELETE");
        Assert.AreEqual("/hello/{id}", endpointDict["Pattern"]);
        Assert.AreEqual("<CreateRouteEndpoint>b__0_0", endpointDict["Handler"]);
        Assert.IsFalse(endpointDict.ContainsKey("Produces"));
    }

    [TestMethod]
    public void GetRoutes_WithDevEndpoint_ReturnsNoEndpointData()
    {
        // arrange
        var options = Options.Create(new DevViewOptions());
        var endpoints = new List<Endpoint>
        {
            EndpointDataSourceFactory.CreateRouteEndpoint("GET", "/dev/info", "DevHandler", typeof(TestResponse))
        };

        var endpointSource = new FakeEndpointDataSource(endpoints);

        // act
        var result = RoutesEndpoint.GetRoutes(endpointSource, options) as IResult;

        // assert
        Assert.IsNotNull(result);
        var jsonResult = result as JsonHttpResult<IEnumerable<Dictionary<string, object>>>;
        Assert.IsNotNull(jsonResult);
        Assert.AreEqual(0, jsonResult.Value!.Count());
    }

    [TestMethod]
    public void GetRoutes_WithIncludeDebugDetails_ReturnsExpectedJsonResult()
    {
        // arrange
        var options = Options.Create(new DevViewOptions { IncludeRouteDebugDetails = true });
        var endpoints = new List<Endpoint>
        {
            EndpointDataSourceFactory.CreateRouteEndpoint("GET", "/hello", "TestHandler", typeof(TestResponse))
        };

        var endpointSource = new FakeEndpointDataSource(endpoints);

        // act
        var result = RoutesEndpoint.GetRoutes(endpointSource, options) as IResult;

        // assert
        Assert.IsNotNull(result);
        var jsonResult = result as JsonHttpResult<IEnumerable<Dictionary<string, object>>>;
        Assert.IsNotNull(jsonResult);
        var endpointDict = jsonResult.Value!.First();
        var methods = endpointDict["Method"] as string[];
        CollectionAssert.Contains(methods, "GET");
        Assert.AreEqual("/hello", endpointDict["Pattern"]);
        var metadataType = endpointDict["MetadataTypes"];
        Assert.IsNotNull(metadataType);
    }

    [TestMethod]
    public void GetRoutes_WithTags_ReturnsExpectedJsonResult()
    {
        // arrange
        var options = Options.Create(new DevViewOptions());
        var endpoints = new List<Endpoint>
        {
            EndpointDataSourceFactory.CreateRouteEndpoint("GET", "/hello", "TestHandler", ["tag1", "tag2"])
        };

        var endpointSource = new FakeEndpointDataSource(endpoints);

        // act
        var result = RoutesEndpoint.GetRoutes(endpointSource, options) as IResult;

        // assert
        Assert.IsNotNull(result);
        var jsonResult = result as JsonHttpResult<IEnumerable<Dictionary<string, object>>>;
        Assert.IsNotNull(jsonResult);
        var endpointDict = jsonResult.Value!.First();
        var methods = endpointDict["Method"] as string[];
        CollectionAssert.Contains(methods, "GET");
        Assert.AreEqual("/hello", endpointDict["Pattern"]);
        var tags = endpointDict["Tags"] as string[];
        CollectionAssert.Contains(tags, "tag2");
    }
}
