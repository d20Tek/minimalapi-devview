using D20Tek.MinimalApi.DevView.Endpoints.Dependencies;
using D20Tek.MinimalApi.DevView.UnitTests.Fakes;
using Microsoft.Extensions.DependencyInjection;

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
        Assert.AreEqual(6, result.Count());
        Assert.IsTrue(result.Any(x => x.ServiceType == typeof(ITestType)));
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
        Assert.AreEqual(0, result.Count());
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
        Assert.IsTrue(result.Count() >= 80);
        Assert.IsTrue(result.Any(x => x.ServiceType == typeof(ITestType)));
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
        Assert.IsTrue(result.Count() >= 100);
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
        Assert.AreEqual(3, result.Count());
        Assert.IsTrue(result.Any(x => x.ServiceType == typeof(ITestType)));
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
        Assert.AreEqual(0, result.Count());
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
        Assert.AreEqual(2, result.Count());
        Assert.IsTrue(result.Any(x => x.ServiceType == typeof(ITestType)));
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
        Assert.AreEqual(0, result.Count());
    }
}
