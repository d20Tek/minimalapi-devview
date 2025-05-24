using D20Tek.MinimalApi.DevView.Endpoints.Dependencies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace D20Tek.MinimalApi.DevView.UnitTests.Endpoints;

[TestClass]
public class DependenciesEndpointTests
{
    [TestMethod]
    public void GetDependencyInfo_BasicServiceDefintions_ReturnsExpectedJsonResult()
    {
        // arrange
        var app = CreateBasicWebApp();
        HttpContext context = CreateContext(app);

        // act 
        var result = DependenciesEndpoint.GetDependencyInfo(context);

        // assert
        Assert.IsNotNull(result);
        var jsonResult = result as JsonHttpResult<DependenciesEndpoint.DependencyInfo[]>;
        Assert.IsNotNull(jsonResult);
        var dependencies = jsonResult.Value!;
        Assert.IsTrue(dependencies.Any(d => d.ServiceType == typeof(IHost).Name));
        Assert.IsTrue(dependencies.Any(d => d.ServiceType == typeof(ILoggerFactory).Name));
        Assert.IsTrue(dependencies.Any(d => d.ServiceType == typeof(ActivitySource).Name));
        Assert.IsTrue(dependencies.Any(d => d.ServiceType == typeof(ConsoleFormatter).Name));
    }

    [TestMethod]
    public void GetDependencyInfo_WithKeyedServices_ReturnsExpectedJsonResult()
    {
        // arrange
        var app = CreateKeyedServicesWebApp();
        HttpContext context = CreateContext(app);

        // act 
        var result = DependenciesEndpoint.GetDependencyInfo(context);

        // assert
        Assert.IsNotNull(result);
        var jsonResult = result as JsonHttpResult<DependenciesEndpoint.DependencyInfo[]>;
        Assert.IsNotNull(jsonResult);
        var dependencies = jsonResult.Value!;
        Assert.IsTrue(dependencies.Any(d => d.ServiceType == typeof(ITestType).Name));
    }

    [TestMethod]
    public void DependencyInfo_Creation_ReturnsExpectedValues()
    {
        // arrange
        var di = new DependenciesEndpoint.DependencyInfo("Test", null, "", null);

        // act
        var result = di with { ServiceType = "Test", Implementation = "Func", Lifetime = "Singleton", AssemblyName = "assembly" };

        // assert
        Assert.AreEqual("Test", result.ServiceType);
        Assert.AreEqual("Func", result.Implementation);
        Assert.AreEqual("Singleton", result.Lifetime);
        Assert.AreEqual("assembly", result.AssemblyName);
    }

    private static WebApplication CreateBasicWebApp()
    {
        var builder = WebApplication.CreateSlimBuilder(new WebApplicationOptions { EnvironmentName = "Development" });
        builder.Services.AddDevView(builder.Configuration);

        return builder.Build();
    }

    private static WebApplication CreateKeyedServicesWebApp()
    {
        var builder = WebApplication.CreateSlimBuilder(new WebApplicationOptions { EnvironmentName = "Development" });
        builder.Services.AddKeyedScoped<ITestType, TestType>("key")
                        .AddKeyedSingleton<ITestType>("key", new TestType())
                        .AddKeyedScoped<ITestType>("key", [ExcludeFromCodeCoverage](sp, k) => new TestType())
                        .AddDevView(builder.Configuration);

        return builder.Build();
    }

    private static HttpContext CreateContext(WebApplication app) =>
        new DefaultHttpContext
        {
            RequestServices = app.Services
        };

    internal interface ITestType;

    internal class TestType : ITestType { }
}
