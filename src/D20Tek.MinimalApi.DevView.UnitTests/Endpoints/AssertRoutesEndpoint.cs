using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace D20Tek.MinimalApi.DevView.UnitTests.Endpoints;

internal static class AssertRoutesEndpoint
{
    public static Dictionary<string, object?> AssertValid(
        this IResult result,
        string expectedMethod = "GET",
        string expectedPattern = "/hello",
        string expectedHandler = "<CreateRouteEndpoint>b__1_0")
    {
        Assert.IsNotNull(result);
        var jsonResult = result as JsonHttpResult<IEnumerable<Dictionary<string, object?>>>;
        Assert.IsNotNull(jsonResult);
        var endpointDict = jsonResult.Value!.First();
        var methods = endpointDict["Method"] as string[];
        CollectionAssert.Contains(methods, expectedMethod);
        Assert.AreEqual(expectedPattern, endpointDict["Pattern"]);
        Assert.AreEqual(expectedHandler, endpointDict["Handler"]);

        return endpointDict;
    }

    public static void AssertProducesOnly(
        this Dictionary<string, object?> endpointDict,
        string expectedProduces = "StatusCode: 200, ContentTypes: application/json, Type: D20Tek.MinimalApi.DevView.UnitTests.Endpoints.RoutesEndpointTests+TestResponse")
    {
        var produces = endpointDict["Produces"] as string[];
        Assert.HasCount(1, produces!);
        CollectionAssert.Contains(produces, expectedProduces);
    }

    public static void AssertEmpty(this IResult result)
    {
        Assert.IsNotNull(result);
        var jsonResult = result as JsonHttpResult<IEnumerable<Dictionary<string, object?>>>;
        Assert.IsNotNull(jsonResult);
        Assert.AreEqual(0, jsonResult.Value!.Count());
    }

    public static void AssertValidComplex(this IResult result)
    {
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
}
