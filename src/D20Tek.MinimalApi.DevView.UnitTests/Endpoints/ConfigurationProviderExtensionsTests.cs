using D20Tek.MinimalApi.DevView.Endpoints.Configs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.CommandLine;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration.Memory;

namespace D20Tek.MinimalApi.DevView.UnitTests.Endpoints;

[TestClass]
public partial class ConfigurationProviderExtensionsTests
{
    [TestMethod]
    public void GetFriendlyName_WithJsonFile_ReturnsAppSettings()
    {
        // arrange
        var provider = new JsonConfigurationProvider(new() { Path = "appsettings.json" });

        // act
        var result = provider.GetFriendlyName();

        // assert
        Assert.AreEqual("Appsettings (appsettings.json)", result);
    }

    [TestMethod]
    public void GetFriendlyName_WithUserSecrets_ReturnsText()
    {
        // arrange
        var root = new ConfigurationBuilder().AddUserSecrets("test-id").Build();
        var provider = root.Providers.First();

        // act
        var result = provider.GetFriendlyName();

        // assert
        Assert.Contains("secrets.json", result);
    }

    [TestMethod]
    public void GetFriendlyName_WithEnvVariables_ReturnsText()
    {
        // arrange
        var provider = new EnvironmentVariablesConfigurationProvider();

        // act
        var result = provider.GetFriendlyName();

        // assert
        Assert.AreEqual("Environment Variable", result);
    }

    [TestMethod]
    public void GetFriendlyName_WithCommandLine_ReturnsText()
    {
        // arrange
        var provider = new CommandLineConfigurationProvider(["arg1=foo"]);

        // act
        var result = provider.GetFriendlyName();

        // assert
        Assert.AreEqual("Command Line", result);
    }

    [TestMethod]
    public void GetFriendlyName_WithInMemory_ReturnsText()
    {
        // arrange
        var provider = new MemoryConfigurationProvider(new());

        // act
        var result = provider.GetFriendlyName();

        // assert
        Assert.AreEqual("In-Memory", result);
    }

    [TestMethod]
    public void GetFriendlyName_WithCustomProvider_ReturnsType()
    {
        // arrange
        var provider = new TestProvider();

        // act
        var result = provider.GetFriendlyName();

        // assert
        Assert.AreEqual("TestProvider", result);
    }
}
