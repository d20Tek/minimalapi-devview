using D20Tek.MinimalApi.DevView.Endpoints.Dependencies;

namespace D20Tek.MinimalApi.DevView.UnitTests.Endpoints;

[TestClass]
public class DependencyInfoTests
{
    [TestMethod]
    public void DependencyInfo_Creation_ReturnsExpectedValues()
    {
        // arrange
        var di = new DependencyInfo("Test", null, "", null);

        // act
        var result = di with
        {
            ServiceType = "Test",
            Implementation = "Func",
            Lifetime = "Singleton",
            AssemblyName = "assembly"
        };

        // assert
        Assert.AreEqual("Test", result.ServiceType);
        Assert.AreEqual("Func", result.Implementation);
        Assert.AreEqual("Singleton", result.Lifetime);
        Assert.AreEqual("assembly", result.AssemblyName);
    }
}
