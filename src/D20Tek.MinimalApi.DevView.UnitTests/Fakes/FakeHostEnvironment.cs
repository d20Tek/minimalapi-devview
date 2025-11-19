using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;

namespace D20Tek.MinimalApi.DevView.UnitTests.Fakes;

[ExcludeFromCodeCoverage]
internal class FakeHostEnvironment(
    string environmentName = "Development",
    string applicationName = "Test Host",
    string contentRootPath = "/www_root") : IHostEnvironment, IWebHostEnvironment
{
    public string EnvironmentName { get; set; } = environmentName;

    public string ApplicationName { get; set; } = applicationName;

    [ExcludeFromCodeCoverage]
    public string ContentRootPath { get; set; } = contentRootPath;

    [ExcludeFromCodeCoverage]
    public IFileProvider ContentRootFileProvider { get; set; } = null!;

    [ExcludeFromCodeCoverage]
    public string WebRootPath { get; set; } = contentRootPath;

    [ExcludeFromCodeCoverage]
    public IFileProvider WebRootFileProvider { get; set; } = null!;
}
