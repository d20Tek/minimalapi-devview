using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace D20Tek.MinimalApi.DevView.UnitTests.Fakes;

internal static class WebApplicationFactory
{
    public static WebApplication CreateBasicWebApp()
    {
        var builder = WebApplication.CreateSlimBuilder(new WebApplicationOptions { EnvironmentName = "Development" });
        builder.Services.AddDevView(builder.Configuration);

        return builder.Build();
    }

    public static WebApplication CreateKeyedServicesWebApp()
    {
        var builder = WebApplication.CreateSlimBuilder(new WebApplicationOptions { EnvironmentName = "Development" });
        builder.Services.AddKeyedScoped<ITestType, TestType>("key")
                        .AddKeyedSingleton<ITestType>("key", new TestType())
                        .AddKeyedScoped<ITestType>("key", [ExcludeFromCodeCoverage] (sp, k) => new TestType())
                        .AddDevView(builder.Configuration);

        return builder.Build();
    }
}

internal interface ITestType;

internal class TestType : ITestType { }
