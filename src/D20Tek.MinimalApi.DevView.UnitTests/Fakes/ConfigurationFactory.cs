using Microsoft.Extensions.Configuration;

namespace D20Tek.MinimalApi.DevView.UnitTests.Fakes;

internal static class ConfigurationFactory
{
    public static IConfigurationRoot CreateDefaultConfig()
    {
        var builder = new ConfigurationBuilder();

        builder.AddJsonFile("appsettings.json")
               .AddJsonFile("appsettings.Development.json")
               .AddEnvironmentVariables()
               .AddCommandLine([]);

        builder.AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["TestKey"] = "TestValue",
            ["ASPNETCORE_URLS"] = "https://localhost:4500; http://localhost:1500"
        });

        return builder.Build();
    }

    public static IConfigurationRoot CreateConfigWithCommandLine()
    {
        var builder = new ConfigurationBuilder();

        builder.AddJsonFile("appsettings.json")
               .AddJsonFile("appsettings.Development.json")
               .AddEnvironmentVariables()
               .AddCommandLine(["arg1=foo", "arg2=bar"]);

        builder.AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["TestKey"] = "TestValue",
            ["ASPNETCORE_URLS"] = "https://localhost:4500; http://localhost:1500"
        });

        return builder.Build();
    }
}
