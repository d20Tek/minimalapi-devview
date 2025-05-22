using D20Tek.MinimalApi.DevView.UnitTests.Fakes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace D20Tek.MinimalApi.DevView.UnitTests;

[TestClass]
public class DependencyInjectionTests
{
    [TestMethod]
    public void AddDevView_RegisterServices()
    {
        // arrange
        var services = new ServiceCollection();
        var builder = new ConfigurationBuilder().Build();

        // act
        var result = services.AddDevView(builder);

        // assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Any(x => x.ServiceType == typeof(IConfigureOptions<DevViewOptions>)));
    }

    [TestMethod]
    public void UseDevView_MapsEndpoints()
    {
        // arrange
        var app = WebApplication.CreateSlimBuilder(new WebApplicationOptions { EnvironmentName = "Development" })
                                .Build();

        // act
        var result = app.UseDevView();

        //assert
        Assert.IsNotNull(result);

        var endpointSource = result.ApplicationServices.GetRequiredService<EndpointDataSource>();
        Assert.AreEqual(2, endpointSource.Endpoints.Count);
        Assert.IsTrue(endpointSource.Endpoints.Any(x => x.DisplayName == "HTTP: GET /dev/info"));
        Assert.IsTrue(endpointSource.Endpoints.Any(x => x.DisplayName == "HTTP: GET /dev/routes => GetRoutes"));
    }

    [TestMethod]
    public void UseDevView_InProd_DoesNotMapEndpoints()
    {
        // arrange
        var app = WebApplication.CreateSlimBuilder()
                                .Build();

        // act
        var result = app.UseDevView();

        //assert
        Assert.IsNotNull(result);

        var endpointSource = result.ApplicationServices.GetRequiredService<EndpointDataSource>();
        Assert.AreEqual(0, endpointSource.Endpoints.Count);
    }

    [TestMethod]
    public void UseDevView_WithOptionOverride_MapsEndpoints()
    {
        // arrange
        var app = WebApplication.CreateSlimBuilder(new WebApplicationOptions { EnvironmentName = "Development" })
                                .Build();

        // act
        var result = app.UseDevView(o => o.BasePath = "/test-dev");

        //assert
        Assert.IsNotNull(result);

        var endpointSource = result.ApplicationServices.GetRequiredService<EndpointDataSource>();
        Assert.AreEqual(2, endpointSource.Endpoints.Count);
        Assert.IsTrue(endpointSource.Endpoints.Any(x => x.DisplayName == "HTTP: GET /test-dev/info"));
        Assert.IsTrue(endpointSource.Endpoints.Any(x => x.DisplayName == "HTTP: GET /test-dev/routes => GetRoutes"));
    }
}
