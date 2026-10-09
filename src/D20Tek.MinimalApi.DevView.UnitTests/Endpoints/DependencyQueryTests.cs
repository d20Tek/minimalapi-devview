using D20Tek.MinimalApi.DevView.Endpoints.Dependencies;

namespace D20Tek.MinimalApi.DevView.UnitTests.Endpoints;

[TestClass]
public class DependencyQueryTests
{
    [TestMethod]
    public void ApplyFilters_WithNamespace_ReturnsFilteredList()
    {
        // arrange
        var app = WebApplicationFactory.CreateKeyedServicesWebApp();
        var provider = app.Services.GetRequiredService<IRegisteredServicesProvider>();
        var query = new DependencyQuery { Implementation = "D20Tek" };

        // act
        var result = query.ApplyFilters(provider.Services);

        // assert
        Assert.IsNotNull(result);
        Assert.HasCount(7, result);
        Assert.Contains(x => x.ServiceType == typeof(ITestType), result);
    }

    [TestMethod]
    public void ApplyFilters_WithFakeNamespace_ReturnsEmptyList()
    {
        // arrange
        var app = WebApplicationFactory.CreateKeyedServicesWebApp();
        var provider = app.Services.GetRequiredService<IRegisteredServicesProvider>();
        var query = new DependencyQuery { Implementation = "Bogus" };

        // act
        var result = query.ApplyFilters(provider.Services);

        // assert
        Assert.IsNotNull(result);
        Assert.IsEmpty(result);
    }

    [TestMethod]
    public void ApplyFilters_WithLifetime_ReturnsFilteredList()
    {
        // arrange
        var app = WebApplicationFactory.CreateKeyedServicesWebApp();
        var provider = app.Services.GetRequiredService<IRegisteredServicesProvider>();
        var query = new DependencyQuery { Lifetime = "Singleton" };

        // act
        var result = query.ApplyFilters(provider.Services);

        // assert
        Assert.IsNotNull(result);
        Assert.IsGreaterThanOrEqualTo(80, result.Count());
        Assert.Contains(x => x.ServiceType == typeof(ITestType), result);
    }

    [TestMethod]
    public void ApplyFilters_WithFakeLifetime_ReturnsEntireList()
    {
        // arrange
        var app = WebApplicationFactory.CreateKeyedServicesWebApp();
        var provider = app.Services.GetRequiredService<IRegisteredServicesProvider>();
        var query = new DependencyQuery { Lifetime = "Bogus" };

        // act
        var result = query.ApplyFilters(provider.Services);

        // assert
        Assert.IsNotNull(result);
        Assert.IsGreaterThanOrEqualTo(100, result.Count());
    }

    [TestMethod]
    public void ApplyFilters_WithServiceContains_ReturnsFilteredList()
    {
        // arrange
        var app = WebApplicationFactory.CreateKeyedServicesWebApp();
        var provider = app.Services.GetRequiredService<IRegisteredServicesProvider>();
        var query = new DependencyQuery { ServiceType = "ITestType" };

        // act
        var result = query.ApplyFilters(provider.Services);

        // assert
        Assert.IsNotNull(result);
        Assert.HasCount(3, result);
        Assert.Contains(x => x.ServiceType == typeof(ITestType), result);
    }

    [TestMethod]
    public void ApplyFilters_WithFakeServiceContains_ReturnsEmptyList()
    {
        // arrange
        var app = WebApplicationFactory.CreateKeyedServicesWebApp();
        var provider = app.Services.GetRequiredService<IRegisteredServicesProvider>();
        var query = new DependencyQuery { ServiceType = "Bogus" };

        // act
        var result = query.ApplyFilters(provider.Services);

        // assert
        Assert.IsNotNull(result);
        Assert.IsEmpty(result);
    }

    [TestMethod]
    public void ApplyFilters_WithAssembly_ReturnsFilteredList()
    {
        // arrange
        var app = WebApplicationFactory.CreateKeyedServicesWebApp();
        var provider = app.Services.GetRequiredService<IRegisteredServicesProvider>();
        var query = new DependencyQuery { Assembly = "UnitTests" };

        // act
        var result = query.ApplyFilters(provider.Services);

        // assert
        Assert.IsNotNull(result);
        Assert.HasCount(2, result);
        Assert.Contains(x => x.ServiceType == typeof(ITestType), result);
    }

    [TestMethod]
    public void ApplyFilters_WithFakeAssembly_ReturnsEmptyList()
    {
        // arrange
        var app = WebApplicationFactory.CreateKeyedServicesWebApp();
        var provider = app.Services.GetRequiredService<IRegisteredServicesProvider>();
        var query = new DependencyQuery { Assembly = "Bogus" };

        // act
        var result = query.ApplyFilters(provider.Services);

        // assert
        Assert.IsNotNull(result);
        Assert.IsEmpty(result);
    }
}
