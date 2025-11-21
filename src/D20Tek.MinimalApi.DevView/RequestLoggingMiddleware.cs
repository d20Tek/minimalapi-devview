using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace D20Tek.MinimalApi.DevView;

public sealed class RequestLoggingMiddleware(
    RequestDelegate next,
    ILogger<RequestLoggingMiddleware> logger,
    IOptions<DevViewOptions> options)
{
    private const double _millisec = 1000.0;
    private const string _logEntry = "--> {method} {path}";
    private const string _logExit = "<-- {status} ({duration}ms)";

    private readonly RequestDelegate _next = next;
    private readonly ILogger<RequestLoggingMiddleware> _logger = logger;
    private readonly DevViewOptions _options = options.Value;

    public async Task InvokeAsync(HttpContext context)
    {
        if (_options.EnableLogging is false)
        {
            await _next(context);
            return;
        }

        var start = Stopwatch.GetTimestamp();
        _logger.Log(_options.LogLevel, _logEntry, context.Request.Method, context.Request.Path);

        await _next(context);

        var elapsed = (Stopwatch.GetTimestamp() - start) * _millisec / Stopwatch.Frequency;
        _logger.Log(_options.LogLevel, _logExit, context.Response.StatusCode, elapsed);
    }
}
