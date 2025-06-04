using D20Tek.MinimalApi.DevView.Endpoints.Configs;
using D20Tek.MinimalApi.DevView.UnitTests.Fakes;

namespace D20Tek.MinimalApi.DevView.UnitTests.Endpoints;

[TestClass]
public class ConfigQueryTests
{
    private static readonly List<ConfigInfo> _configEntries =
    [
        new("Test1", "foo", "appsettings.json", false, "string"),
        new("Test2", "False", "Environment Variables", false, "bool"),
        new("DevView:LogLevel", "Default", "appsettings.json", false, "string")
    ];

    [TestMethod]
    public void ApplyFilters_WithKeyName_ReturnsFilteredList()
    {
        // arrange
        var query = new ConfigQuery { KeyName = "DevView" };

        // act
        var result = query.ApplyFilters(_configEntries);

        // assert
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Count());
        Assert.IsTrue(result.Any(x => x.Key == "DevView:LogLevel"));
    }

    [TestMethod]
    public void ApplyFilters_WithSource_ReturnsFilteredList()
    {
        // arrange
        var query = new ConfigQuery { Source = "appsettings.json" };

        // act
        var result = query.ApplyFilters(_configEntries);

        // assert
        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Count());
        Assert.IsTrue(result.Any(x => x.Key == "Test1"));
        Assert.IsTrue(result.Any(x => x.Key == "DevView:LogLevel"));
    }

    [TestMethod]
    public void ApplyFilters_WithValueType_ReturnsFilteredList()
    {
        // arrange
        var query = new ConfigQuery { ValueType = "bool" };

        // act
        var result = query.ApplyFilters(_configEntries);

        // assert
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Count());
        Assert.IsTrue(result.Any(x => x.Key == "Test2"));
    }
}
