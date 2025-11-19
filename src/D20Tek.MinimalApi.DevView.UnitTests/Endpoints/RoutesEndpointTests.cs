using D20Tek.MinimalApi.DevView.Endpoints.Routes;
using D20Tek.MinimalApi.DevView.UnitTests.Fakes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

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
        var context = new DefaultHttpContext();
        var endpoints = new List<Endpoint>
        {
            EndpointDataSourceFactory.CreateRouteEndpoint("GET", "/hello", "TestHandler", typeof(TestResponse))
        };

        var endpointSource = new FakeEndpointDataSource(endpoints);

        // act
        var result = RoutesEndpoint.GetRoutes(endpointSource, context, options) as IResult;

        // assert
        Assert.IsNotNull(result);
        var jsonResult = result as JsonHttpResult<IEnumerable<Dictionary<string, object?>>>;
        Assert.IsNotNull(jsonResult);
        var endpointDict = jsonResult.Value!.First();
        var methods = endpointDict["Method"] as string[];
        CollectionAssert.Contains(methods, "GET");
        Assert.AreEqual("/hello", endpointDict["Pattern"]);
        Assert.AreEqual("<CreateRouteEndpoint>b__1_0", endpointDict["Handler"]);
        var produces = endpointDict["Produces"] as string[];
        Assert.HasCount(1, produces!);
        CollectionAssert.Contains(
            produces,
            "StatusCode: 200, ContentTypes: application/json, Type: D20Tek.MinimalApi.DevView.UnitTests.Endpoints.RoutesEndpointTests+TestResponse");
    }

    [TestMethod]
    public void GetRoutes_WithMultipleEndpoints_ReturnsExpectedJsonResult()
    {
        // arrange
        var options = Options.Create(new DevViewOptions());
        var context = new DefaultHttpContext();
        var endpoints = new List<Endpoint>
        {
            EndpointDataSourceFactory.CreateRouteEndpoint("GET", "/hello", "TestHandler", typeof(TestResponse)),
            EndpointDataSourceFactory.CreateRouteEndpoint("DELETE", "/hello/{id}", "DeleteHandler")
        };

        var endpointSource = new FakeEndpointDataSource(endpoints);

        // act
        var result = RoutesEndpoint.GetRoutes(endpointSource, context, options) as IResult;

        // assert
        Assert.IsNotNull(result);
        var jsonResult = result as JsonHttpResult<IEnumerable<Dictionary<string, object?>>>;
        Assert.IsNotNull(jsonResult);
        var endpointDict = jsonResult.Value!.First();
        var methods = endpointDict["Method"] as string[];
        CollectionAssert.Contains(methods, "GET");
        Assert.AreEqual("/hello", endpointDict["Pattern"]);
        Assert.AreEqual("<CreateRouteEndpoint>b__1_0", endpointDict["Handler"]);
        var produces = endpointDict["Produces"] as string[];
        Assert.HasCount(1, produces!);
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
        var context = new DefaultHttpContext();
        var endpoints = new List<Endpoint>
        {
            EndpointDataSourceFactory.CreateRouteEndpoint("GET", "/dev/info", "DevHandler", typeof(TestResponse))
        };

        var endpointSource = new FakeEndpointDataSource(endpoints);

        // act
        var result = RoutesEndpoint.GetRoutes(endpointSource, context, options) as IResult;

        // assert
        Assert.IsNotNull(result);
        var jsonResult = result as JsonHttpResult<IEnumerable<Dictionary<string, object?>>>;
        Assert.IsNotNull(jsonResult);
        Assert.AreEqual(0, jsonResult.Value!.Count());
    }

    [TestMethod]
    public void GetRoutes_WithIncludeDebugDetails_ReturnsExpectedJsonResult()
    {
        // arrange
        var options = Options.Create(new DevViewOptions { IncludeRouteDebugDetails = true });
        var context = new DefaultHttpContext();
        var endpoints = new List<Endpoint>
        {
            EndpointDataSourceFactory.CreateRouteEndpoint("GET", "/hello", "TestHandler", typeof(TestResponse))
        };

        var endpointSource = new FakeEndpointDataSource(endpoints);

        // act
        var result = RoutesEndpoint.GetRoutes(endpointSource, context, options) as IResult;

        // assert
        Assert.IsNotNull(result);
        var jsonResult = result as JsonHttpResult<IEnumerable<Dictionary<string, object?>>>;
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
        var context = new DefaultHttpContext();
        var endpoints = new List<Endpoint>
        {
            EndpointDataSourceFactory.CreateRouteEndpoint("GET", "/hello", "TestHandler", ["tag1", "tag2"])
        };

        var endpointSource = new FakeEndpointDataSource(endpoints);

        // act
        var result = RoutesEndpoint.GetRoutes(endpointSource, context, options) as IResult;

        // assert
        Assert.IsNotNull(result);
        var jsonResult = result as JsonHttpResult<IEnumerable<Dictionary<string, object?>>>;
        Assert.IsNotNull(jsonResult);
        var endpointDict = jsonResult.Value!.First();
        var methods = endpointDict["Method"] as string[];
        CollectionAssert.Contains(methods, "GET");
        Assert.AreEqual("/hello", endpointDict["Pattern"]);
        var tags = endpointDict["Tags"] as string[];
        CollectionAssert.Contains(tags, "tag2");
        var endpointName = endpointDict["Name"] as string;
        Assert.AreEqual("TestHandler", endpointName);
    }

    [TestMethod]
    public void GetRoutes_WithEndpointName_ReturnsExpectedJsonResult()
    {
        // arrange
        var options = Options.Create(new DevViewOptions());
        var context = new DefaultHttpContext();
        var endpoints = new List<Endpoint>
        {
            EndpointDataSourceFactory.CreateRouteEndpoint("GET", "/hello", "TestHandler", ["tag1", "tag2"])
        };

        var endpointSource = new FakeEndpointDataSource(endpoints);

        // act
        var result = RoutesEndpoint.GetRoutes(endpointSource, context, options) as IResult;

        // assert
        Assert.IsNotNull(result);
        var jsonResult = result as JsonHttpResult<IEnumerable<Dictionary<string, object?>>>;
        Assert.IsNotNull(jsonResult);
        var endpointDict = jsonResult.Value!.First();
        var methods = endpointDict["Method"] as string[];
        CollectionAssert.Contains(methods, "GET");
        Assert.AreEqual("/hello", endpointDict["Pattern"]);
        var endpointName = endpointDict["Name"] as string;
        Assert.AreEqual("TestHandler", endpointName);
    }

    [TestMethod]
    public void GetRoutes_WithIResultOnly_ReturnsExpectedJsonResult()
    {
        // arrange
        var options = Options.Create(new DevViewOptions());
        var context = new DefaultHttpContext();
        var endpoints = new List<Endpoint>
        {
            EndpointDataSourceFactory.CreateRouteEndpoint("GET", "/hello", "TestHandler", typeof(IResult))
        };

        var endpointSource = new FakeEndpointDataSource(endpoints);

        // act
        var result = RoutesEndpoint.GetRoutes(endpointSource, context, options) as IResult;

        // assert
        Assert.IsNotNull(result);
        var jsonResult = result as JsonHttpResult<IEnumerable<Dictionary<string, object?>>>;
        Assert.IsNotNull(jsonResult);
        var endpointDict = jsonResult.Value!.First();
        var produces = endpointDict["Produces"] as string[];
        Assert.HasCount(1, produces!);
        CollectionAssert.Contains(
            produces,
            "StatusCode: 200, ContentTypes: application/json, Type: Microsoft.AspNetCore.Http.IResult");
    }

    [TestMethod]
    public void GetRoutes_WithDuplicateProduces_ReturnsExpectedJsonResult()
    {
        // arrange
        var options = Options.Create(new DevViewOptions());
        var context = new DefaultHttpContext();
        var endpoints = new List<Endpoint>
        {
            EndpointDataSourceFactory.CreateRouteEndpoint(
                "GET", "/hello", "TestHandler", typeof(IResult), typeof(TestResponse))
        };

        var endpointSource = new FakeEndpointDataSource(endpoints);

        // act
        var result = RoutesEndpoint.GetRoutes(endpointSource, context, options) as IResult;

        // assert
        Assert.IsNotNull(result);
        var jsonResult = result as JsonHttpResult<IEnumerable<Dictionary<string, object?>>>;
        Assert.IsNotNull(jsonResult);
        var endpointDict = jsonResult.Value!.First();
        var produces = endpointDict["Produces"] as string[];
        Assert.HasCount(1, produces!);
        CollectionAssert.Contains(
            produces,
            "StatusCode: 200, ContentTypes: application/json, Type: D20Tek.MinimalApi.DevView.UnitTests.Endpoints.RoutesEndpointTests+TestResponse");
    }

    [TestMethod]
    public void GetRoutes_WithNoRoutePattern_ReturnsExpectedJsonResult()
    {
        // arrange
        var options = Options.Create(new DevViewOptions());
        var context = new DefaultHttpContext();
        var endpoints = new List<Endpoint>
        {
            EndpointDataSourceFactory.CreateNoPatternEndpoint("GET")
        };

        var endpointSource = new FakeEndpointDataSource(endpoints);

        // act
        var result = RoutesEndpoint.GetRoutes(endpointSource, context, options) as IResult;

        // assert
        Assert.IsNotNull(result);
        var jsonResult = result as JsonHttpResult<IEnumerable<Dictionary<string, object?>>>;
        Assert.IsNotNull(jsonResult);
        var endpointDict = jsonResult.Value!.First();
        Assert.AreEqual("none", endpointDict["Pattern"]);
    }

    [TestMethod]
    public void GetRoutes_WithEndpointsFilter_ReturnsExpectedJsonResult()
    {
        // arrange
        var options = Options.Create(new DevViewOptions());
        var context = new DefaultHttpContext();
        context.Request.Query = new QueryCollection(new Dictionary<string, StringValues>
        {
            ["endpointName"] = "TestHandler",
        });

        var endpoints = new List<Endpoint>
        {
            EndpointDataSourceFactory.CreateRouteEndpoint("GET", "/hello", "TestHandler", []),
            EndpointDataSourceFactory.CreateRouteEndpoint("DELETE", "/hello/{id}", "DeleteHandler", [])
        };

        var endpointSource = new FakeEndpointDataSource(endpoints);

        // act
        var result = RoutesEndpoint.GetRoutes(endpointSource, context, options) as IResult;

        // assert
        Assert.IsNotNull(result);
        var jsonResult = result as JsonHttpResult<IEnumerable<Dictionary<string, object?>>>;
        Assert.IsNotNull(jsonResult);
        Assert.AreEqual(1, jsonResult.Value!.Count());
        var endpointDict = jsonResult.Value!.First();
        var methods = endpointDict["Method"] as string[];
        CollectionAssert.Contains(methods, "GET");
        Assert.AreEqual("/hello", endpointDict["Pattern"]);
    }
}
