using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace D20Tek.MinimalApi.DevView;

public class RequestLoggingMiddleware
{
    private const double _millisec = 1000.0;
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;
    private readonly DevViewOptions _options;

    public RequestLoggingMiddleware(
        RequestDelegate next,
        ILogger<RequestLoggingMiddleware> logger,
        IOptions<DevViewOptions> options)
    {
        _next = next;
        _logger = logger;
        _options = options.Value;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (_options.EnableLogging is false)
        {
            await _next(context);
            return;
        }

        var start = Stopwatch.GetTimestamp();
        _logger.Log(_options.LogLevel, "--> {method} {path}", context.Request.Method, context.Request.Path);

        await _next(context);

        var elapsed = (Stopwatch.GetTimestamp() - start) * _millisec / Stopwatch.Frequency;
        _logger.Log(_options.LogLevel, "<-- {status} ({duration}ms)", context.Response.StatusCode, elapsed);
    }
}
