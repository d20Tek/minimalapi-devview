using D20Tek.MinimalApi.DevView.UnitTests.Fakes;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;

namespace D20Tek.MinimalApi.DevView.UnitTests;

[TestClass]
public class RequestLoggingMiddlewareTests
{
    private static readonly HttpContext _context = new DefaultHttpContext();
    private static readonly RequestDelegate _next = new([ExcludeFromCodeCoverage] (context) => Task.CompletedTask);


    [TestMethod]
    public async Task InvokeAsync_WithLoggingOff_LogsNothing()
    {
        // arrange
        var options = Options.Create(new DevViewOptions { EnableLogging = false });
        var logger = new FakeLogger<RequestLoggingMiddleware>();
        var middleware = new RequestLoggingMiddleware(_next, logger, options);

        // act
        await middleware.InvokeAsync(_context);

        // assert
        Assert.IsEmpty(logger.Logs);
    }

    [TestMethod]
    public async Task InvokeAsync_WithLoggingOn_LogsOperationStartAndEnd()
    {
        // arrange
        var options = Options.Create(new DevViewOptions());
        var logger = new FakeLogger<RequestLoggingMiddleware>();
        var middleware = new RequestLoggingMiddleware(_next, logger, options);
        var context = HttpContextFactory.CreateConfiguredRequest();

        // act
        await middleware.InvokeAsync(context);

        // assert
        Assert.HasCount(2, logger.Logs);
        Assert.IsTrue(logger.Logs.All(x => x.StartsWith("[Information]")));
        Assert.AreEqual("[Information] --> GET /api/test", logger.Logs[0]);
        Assert.StartsWith("[Information] <-- 200", logger.Logs[1]);
    }

    [TestMethod]
    public async Task InvokeAsync_WithLogLevel_LogsOperationStartAndEnd()
    {
        // arrange
        var options = Options.Create(new DevViewOptions{ LogLevel = LogLevel.Debug });
        var logger = new FakeLogger<RequestLoggingMiddleware>();
        var middleware = new RequestLoggingMiddleware(_next, logger, options);

        // act
        await middleware.InvokeAsync(_context);

        // assert
        Assert.HasCount(2, logger.Logs);
        Assert.IsTrue(logger.Logs.All(x => x.StartsWith("[Debug]")));
    }
}
