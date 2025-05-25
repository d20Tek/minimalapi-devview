using D20Tek.MinimalApi.DevView.Endpoints.Dependencies;
using D20Tek.MinimalApi.DevView.UnitTests.Fakes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System.Diagnostics;

namespace D20Tek.MinimalApi.DevView.UnitTests.Endpoints;

[TestClass]
public class DependenciesEndpointTests
{
    [TestMethod]
    public void GetDependencyInfo_BasicServiceDefintions_ReturnsExpectedJsonResult()
    {
        // arrange
        var app = WebApplicationFactory.CreateBasicWebApp();
        HttpContext context = CreateContext(app);

        // act 
        var result = DependenciesEndpoint.GetDependencyInfo(context);

        // assert
        Assert.IsNotNull(result);
        var jsonResult = result as JsonHttpResult<DependencyInfo[]>;
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
        var app = WebApplicationFactory.CreateKeyedServicesWebApp();
        HttpContext context = CreateContext(app);

        // act 
        var result = DependenciesEndpoint.GetDependencyInfo(context);

        // assert
        Assert.IsNotNull(result);
        var jsonResult = result as JsonHttpResult<DependencyInfo[]>;
        Assert.IsNotNull(jsonResult);
        var dependencies = jsonResult.Value!;
        Assert.IsTrue(dependencies.Any(d => d.ServiceType == typeof(ITestType).Name));
    }

    private static HttpContext CreateContext(WebApplication app) =>
        new DefaultHttpContext
        {
            RequestServices = app.Services
        };
}
