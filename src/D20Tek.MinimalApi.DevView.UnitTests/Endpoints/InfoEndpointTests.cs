using D20Tek.MinimalApi.DevView.Endpoints.Info;
using D20Tek.MinimalApi.DevView.UnitTests.Fakes;
using Microsoft.AspNetCore.Http.HttpResults;

namespace D20Tek.MinimalApi.DevView.UnitTests.Endpoints;

[TestClass]
public class InfoEndpointTests
{
    [TestMethod]
    public void GetDevInfo_ReturnsExpectedJsonResult()
    {
        // arrange
        var env = new FakeHostEnvironment();

        // act
        var result = InfoEndpoint.GetDevInfo(env);

        // assert
        Assert.IsNotNull(result);
        var jsonResult = result as JsonHttpResult<InfoResponse>;
        Assert.IsNotNull(jsonResult);
        var response = jsonResult.Value!;
        Assert.AreEqual("Development", response.Environment);
        Assert.AreEqual("Test Host", response.AppName);
        Assert.AreEqual("15.0.0.0", response.Version);
        Assert.IsTrue(response.StartTime < DateTime.UtcNow);
        Assert.IsGreaterThanOrEqualTo(0, response.UptimeSeconds);
    }

    [TestMethod]
    public void GetDevInfo_WithOtherDetails_ReturnsExpectedJsonResult()
    {
        // arrange
        var env = new FakeHostEnvironment("Test", "MyApp");

        // act
        var result = InfoEndpoint.GetDevInfo(env);

        // assert
        Assert.IsNotNull(result);
        var jsonResult = result as JsonHttpResult<InfoResponse>;
        Assert.IsNotNull(jsonResult);
        var response = jsonResult.Value!;
        Assert.AreEqual("Test", response.Environment);
        Assert.AreEqual("MyApp", response.AppName);
    }
}
