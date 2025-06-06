﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;

namespace D20Tek.MinimalApi.DevView.UnitTests.Fakes;

internal class FakeHostEnvironment : IHostEnvironment, IWebHostEnvironment
{
    public string EnvironmentName { get; set; }

    public string ApplicationName { get; set; }

    [ExcludeFromCodeCoverage]
    public string ContentRootPath { get; set; }

    [ExcludeFromCodeCoverage]
    public IFileProvider ContentRootFileProvider { get; set; } = null!;

    [ExcludeFromCodeCoverage]
    public string WebRootPath { get; set; }

    [ExcludeFromCodeCoverage]
    public IFileProvider WebRootFileProvider { get; set; } = null!;

    public FakeHostEnvironment(
        string environmentName = "Development",
        string applicationName = "Test Host",
        string contentRootPath = "/www_root")
    {
        EnvironmentName = environmentName;
        ApplicationName = applicationName;
        ContentRootPath = contentRootPath;
        WebRootPath = contentRootPath;
    }
}
