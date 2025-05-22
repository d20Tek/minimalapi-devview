using Microsoft.Extensions.Logging;

namespace D20Tek.MinimalApi.DevView.UnitTests;

[TestClass]
public class DevViewOptionsTests
{
    [TestMethod]
    public void CreateOptions_GetsExpectedValues()
    {
        // arrange

        // act
        var options = new DevViewOptions
        {
            BasePath = "/foo",
            EnableLogging = true,
            IncludeRequestBodies = true,
            IncludeRouteDebugDetails = true,
            IncludeRouteMetadata = true,
            LogLevel = LogLevel.Error
        };
    
        // assert
        Assert.IsNotNull(options);
        Assert.AreEqual("/foo", options.BasePath);
        Assert.IsTrue(options.EnableLogging);
        Assert.IsTrue(options.IncludeRequestBodies);
        Assert.IsTrue(options.IncludeRouteDebugDetails);
        Assert.IsTrue(options.IncludeRouteMetadata);
        Assert.AreEqual(LogLevel.Error, options.LogLevel);
    }
}
