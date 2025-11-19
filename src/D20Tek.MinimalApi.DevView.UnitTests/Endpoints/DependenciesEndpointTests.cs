using D20Tek.MinimalApi.DevView.Endpoints.Dependencies;
using D20Tek.MinimalApi.DevView.UnitTests.Fakes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Primitives;
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
        Assert.IsTrue(dependencies.Any(d => d.ServiceType == typeof(IHost).FullName));
        Assert.IsTrue(dependencies.Any(d => d.ServiceType == typeof(ILoggerFactory).FullName));
        Assert.IsTrue(dependencies.Any(d => d.ServiceType == typeof(ActivitySource).FullName));
        Assert.IsTrue(dependencies.Any(d => d.ServiceType == typeof(ConsoleFormatter).FullName));
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
        Assert.IsTrue(dependencies.Any(d => d.ServiceType == typeof(ITestType).FullName));
    }

    [TestMethod]
    public void GetDependencyInfo_WithFilteredServiceType_ReturnsExpectedJsonResult()
    {
        // arrange
        var app = WebApplicationFactory.CreateBasicWebApp();
        var query = new QueryCollection(new Dictionary<string, StringValues>
        {
            ["serviceType"] = "D20Tek",
        });
        HttpContext context = CreateContext(app, query);

        // act 
        var result = DependenciesEndpoint.GetDependencyInfo(context);

        // assert
        Assert.IsNotNull(result);
        var jsonResult = result as JsonHttpResult<DependencyInfo[]>;
        Assert.IsNotNull(jsonResult);
        var dependencies = jsonResult.Value!;
        Assert.IsFalse(dependencies.Any(d => d.ServiceType == typeof(IHost).FullName));
        Assert.IsTrue(dependencies.Any(d => d.ServiceType == typeof(IRegisteredServicesProvider).FullName));
    }

    private static DefaultHttpContext CreateContext(WebApplication app, IQueryCollection? query = null)
    {
        var context = new DefaultHttpContext
        {
            RequestServices = app.Services,

        };

        context.Request.Query = query ?? new QueryCollection();

        return context;
    }
}
