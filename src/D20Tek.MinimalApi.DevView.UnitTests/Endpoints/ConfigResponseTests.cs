using D20Tek.MinimalApi.DevView.Endpoints.Configs;

namespace D20Tek.MinimalApi.DevView.UnitTests.Endpoints;

[TestClass]
public class ConfigResponseTests
{
    [TestMethod]
    public void ProviderInfo_Creation_ReturnsExpectedValues()
    {
        // arrange
        var pi = new ProviderInfo("", 0);

        // act
        var result = pi with
        {
            Name = "Test",
            ProvidedKeys = 5
        };

        // assert
        Assert.AreEqual("Test", result.Name);
        Assert.AreEqual(5, result.ProvidedKeys);
    }

    [TestMethod]
    public void ConfigInfo_Creation_ReturnsExpectedValues()
    {
        // arrange
        var ci = new ConfigInfo("", null, "", false, null);

        // act
        var result = ci with
        {
            Key = "Test",
            Value = "X1",
            Source = "ConnectionString",
            IsSensitive = true,
            ValueType = "string"
        };

        // assert
        Assert.AreEqual("Test", result.Key);
        Assert.AreEqual("X1", result.Value);
        Assert.AreEqual("ConnectionString", result.Source);
        Assert.AreEqual(true, result.IsSensitive);
        Assert.AreEqual("string", result.ValueType);
    }

    [TestMethod]
    public void ConfigSummary_Creation_ReturnsExpectedValues()
    {
        // arrange
        var cs = new ConfigSummary("", [], [], []);

        // act
        var result = cs with
        {
            EnvironmentName = "Dev",
            LoadedJsonFiles = ["appsetting.json", "appsettings.Development.json"],
            Providers = [new("1", 1), new("2", 2), new("three", 3)],
            EffectiveUrls = [ "/url1", "/url2"]
        };

        // assert
        Assert.AreEqual("Dev", result.EnvironmentName);
        Assert.AreEqual(2, result.LoadedJsonFiles.Length);
        Assert.AreEqual(3, result.Providers.Length);
        Assert.AreEqual(2, result.EffectiveUrls.Length);
    }

    [TestMethod]
    public void ConfigResponse_Creation_ReturnsExpectedValues()
    {
        // arrange
        var response = new ConfigResponse(new ConfigSummary("Dev", [], [], []), []);
        var summary = new ConfigSummary(
            "Dev",
            ["appsetting.json", "appsettings.Development.json"],
            [new("1", 1), new("2", 2), new("three", 3)],
            ["/url1", "/url2"]);
        var entries = new List<ConfigInfo>()
        {
            new("Test1", "foo", "appsettings.json", false, "string"),
            new("Test2", "False", "Environment Variables", false, "bool"),
            new("DevView:LogLevel", "Default", "appsettings.json", false, "string")
        };


        // act
        var result = response with
        {
            Summary = summary,
            ConfigDetails = entries
        };

        // assert
        Assert.AreEqual(summary, result.Summary);
        Assert.AreEqual(entries, result.ConfigDetails);
        Assert.AreEqual(3, result.ConfigDetails.Count);
    }
}
