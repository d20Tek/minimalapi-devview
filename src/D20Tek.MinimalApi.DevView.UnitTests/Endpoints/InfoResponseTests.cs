using D20Tek.MinimalApi.DevView.Endpoints.Info;

namespace D20Tek.MinimalApi.DevView.UnitTests.Endpoints;

[TestClass]
public class InfoResponseTests
{
    [TestMethod]
    public void InfoResponse_WithUpdate_ReturnsExpectedValues()
    {
        // arrange
        var pi = new InfoResponse("", "", "", default, 0);
        var startTime = DateTime.Now;

        // act
        var result = pi with
        {
            AppName = "test-app",
            Environment = "dev",
            Version = "1.0.1",
            StartTime = startTime,
            UptimeSeconds = 60
        };

        // assert
        Assert.AreEqual("test-app", result.AppName);
        Assert.AreEqual("dev", result.Environment);
        Assert.AreEqual("1.0.1", result.Version);
        Assert.AreEqual(startTime, result.StartTime);
        Assert.AreEqual(60, result.UptimeSeconds);
    }
}
