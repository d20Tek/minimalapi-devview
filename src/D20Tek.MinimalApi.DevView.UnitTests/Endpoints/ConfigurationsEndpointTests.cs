using D20Tek.MinimalApi.DevView.Endpoints.Configs;
using D20Tek.MinimalApi.DevView.UnitTests.Fakes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System.Diagnostics.CodeAnalysis;

namespace D20Tek.MinimalApi.DevView.UnitTests.Endpoints;

[TestClass]
public class ConfigurationsEndpointTests
{
    private readonly IWebHostEnvironment _hostEnv = new FakeHostEnvironment();
    private readonly HttpContext _defaultContext = CreateContext();
    private readonly IOptions<DevViewOptions> _defaultOptions = Options.Create(new DevViewOptions());

    [TestMethod]
    public void GetConfigInfo_WithDefaultConfig_ReturnsExpectedJsonResult()
    {
        // arrange
        var config = ConfigurationFactory.CreateDefaultConfig();

        // act
        var result = ConfigurationsEndpoint.GetConfigInfo(config, _hostEnv, _defaultContext, _defaultOptions);

        // assert
        Assert.IsNotNull(result);
        var jsonResult = result as JsonHttpResult<ConfigResponse>;
        Assert.IsNotNull(jsonResult);
        var response = jsonResult.Value as ConfigResponse;
        Assert.IsNotNull(response);
        Assert.AreEqual(4, response.Summary.Providers.Length);
        Assert.AreEqual(2, response.Summary.LoadedJsonFiles.Length);
        Assert.IsTrue(response.Summary.LoadedJsonFiles.Any(x => x == "appsettings.json"));
        Assert.AreEqual("Development", response.Summary.EnvironmentName);
        Assert.AreEqual(2, response.Summary.EffectiveUrls.Length);
        Assert.AreEqual(10, response.ConfigDetails.Count);
        Assert.IsTrue(response.ConfigDetails.Any(x => x.Key == "ConnectionStrings:DefaultConnection" && x.Value == "*****"));
        Assert.IsTrue(response.ConfigDetails.Any(x => x.Key == "TestKey"));
    }

    [TestMethod]
    public void GetConfigInfo_WithDefaultConfigButShowingSensitive_ReturnsExpectedJsonResult()
    {
        // arrange
        var config = ConfigurationFactory.CreateDefaultConfig();
        var options = Options.Create(new DevViewOptions { ShowSensitiveConfigData = true, IncludeAllEnvVariables = true });

        // act
        var result = ConfigurationsEndpoint.GetConfigInfo(config, _hostEnv, _defaultContext, options);

        // assert
        Assert.IsNotNull(result);
        var jsonResult = result as JsonHttpResult<ConfigResponse>;
        Assert.IsNotNull(jsonResult);
        var response = jsonResult.Value as ConfigResponse;
        Assert.IsNotNull(response);
        Assert.AreEqual(4, response.Summary.Providers.Length);
        Assert.AreEqual(2, response.Summary.LoadedJsonFiles.Length);
        Assert.AreEqual(2, response.Summary.EffectiveUrls.Length);
        Assert.IsTrue(response.ConfigDetails.Count >= 60);
        Assert.IsTrue(response.ConfigDetails.Any(x => x.Key == "ConnectionStrings:DefaultConnection"));
        Assert.IsFalse(response.ConfigDetails.Any(x => x.Key == "ConnectionStrings:DefaultConnection" && x.Value == "*****"));
    }

    [TestMethod]
    public void GetConfigInfo_WithConfigQuery_ReturnsExpectedJsonResult()
    {
        // arrange
        var config = ConfigurationFactory.CreateDefaultConfig();
        var query = new QueryCollection(new Dictionary<string, StringValues>
        {
            ["keyName"] = "DevView",
        });
        var context = CreateContext(query);

        // act
        var result = ConfigurationsEndpoint.GetConfigInfo(config, _hostEnv, context, _defaultOptions);

        // assert
        Assert.IsNotNull(result);
        var jsonResult = result as JsonHttpResult<ConfigResponse>;
        Assert.IsNotNull(jsonResult);
        var response = jsonResult.Value as ConfigResponse;
        Assert.IsNotNull(response);
        Assert.AreEqual(1, response.Summary.Providers.Length);
        Assert.AreEqual(2, response.Summary.LoadedJsonFiles.Length);
        Assert.IsTrue(response.Summary.LoadedJsonFiles.Any(x => x == "appsettings.json"));
        Assert.AreEqual("Development", response.Summary.EnvironmentName);
        Assert.AreEqual(2, response.Summary.EffectiveUrls.Length);
        Assert.AreEqual(3, response.ConfigDetails.Count);
        Assert.IsFalse(response.ConfigDetails.Any(x => x.Key == "ConnectionStrings:DefaultConnection"));
        Assert.IsFalse(response.ConfigDetails.Any(x => x.Key == "TestKey"));
        Assert.IsTrue(response.ConfigDetails.Any(x => x.Key == "DevView:LogLevel"));
    }

    [TestMethod]
    public void GetConfigInfo_WithCommandLine_ReturnsExpectedJsonResult()
    {
        // arrange
        var config = ConfigurationFactory.CreateConfigWithCommandLine();

        // act
        var result = ConfigurationsEndpoint.GetConfigInfo(config, _hostEnv, _defaultContext, _defaultOptions);

        // assert
        Assert.IsNotNull(result);
        var jsonResult = result as JsonHttpResult<ConfigResponse>;
        Assert.IsNotNull(jsonResult);
        var response = jsonResult.Value as ConfigResponse;
        Assert.IsNotNull(response);
        Assert.AreEqual(5, response.Summary.Providers.Length);
        Assert.AreEqual(2, response.Summary.LoadedJsonFiles.Length);
        Assert.AreEqual(2, response.Summary.EffectiveUrls.Length);
        Assert.AreEqual(12, response.ConfigDetails.Count);
        Assert.IsTrue(response.ConfigDetails.Any(x => x.Key == "arg1"));
        Assert.IsTrue(response.ConfigDetails.Any(x => x.Key == "arg2"));
    }

    [TestMethod]
    public void GetConfigInfo_WithNoEffectiveUrls_ReturnsExpectedJsonResult()
    {
        // arrange
        var config = ConfigurationFactory.CreateConfigWithNoUrls();

        // act
        var result = ConfigurationsEndpoint.GetConfigInfo(config, _hostEnv, _defaultContext, _defaultOptions);

        // assert
        Assert.IsNotNull(result);
        var jsonResult = result as JsonHttpResult<ConfigResponse>;
        Assert.IsNotNull(jsonResult);
        var response = jsonResult.Value as ConfigResponse;
        Assert.IsNotNull(response);
        Assert.AreEqual(4, response.Summary.Providers.Length);
        Assert.AreEqual(2, response.Summary.LoadedJsonFiles.Length);
        Assert.AreEqual(0, response.Summary.EffectiveUrls.Length);
    }

    [TestMethod]
    public void GetConfigInfo_WithNonConfigurationHost_ReturnsEmptyJsonResult()
    {
        // arrange
        var config = new EmptyConfiguration();

        // act
        var result = ConfigurationsEndpoint.GetConfigInfo(config, _hostEnv, _defaultContext, _defaultOptions);

        // assert
        Assert.IsNotNull(result);
        var jsonResult = result as JsonHttpResult<ConfigResponse>;
        Assert.IsNotNull(jsonResult);
        Assert.IsNull(jsonResult.Value);
    }

    private static HttpContext CreateContext(IQueryCollection? query = null)
    {
        var context = new DefaultHttpContext();
        context.Request.Query = query ?? new QueryCollection();

        return context;
    }

    [ExcludeFromCodeCoverage]
    internal class EmptyConfiguration : IConfiguration
    {
        public string? this[string key] 
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public IEnumerable<IConfigurationSection> GetChildren() => throw new NotImplementedException();

        public IChangeToken GetReloadToken() => throw new NotImplementedException();

        public IConfigurationSection GetSection(string key) => throw new NotImplementedException();
    }
}
