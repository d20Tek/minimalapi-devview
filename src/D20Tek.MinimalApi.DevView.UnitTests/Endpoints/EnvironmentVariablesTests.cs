using D20Tek.MinimalApi.DevView.Endpoints.Configs;

namespace D20Tek.MinimalApi.DevView.UnitTests.Endpoints;

[TestClass]
public class EnvironmentVariablesTests
{
    [TestMethod]
    public void IsRelevant_WithRelevantKeyName_ReturnsTrue()
    {
        // arrange

        // act
        var result = EnvironmentVariables.IsRelevant("ASPNETCORE_ENVIRONMENT");

        // assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void IsRelevant_WithIrrelevantKeyName_ReturnsFalse()
    {
        // arrange

        // act
        var result = EnvironmentVariables.IsRelevant("MyKeyName");

        // assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void IsRelevant_WithEmptyKeyName_ReturnsFalse()
    {
        // arrange

        // act
        var result = EnvironmentVariables.IsRelevant(string.Empty);

        // assert
        Assert.IsFalse(result);
    }
}
