using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace D20Tek.MinimalApi.DevView.UnitTests.Fakes;

[ExcludeFromCodeCoverage]
public class FakeLogger<T> : ILogger<T>
{
    private readonly List<string> _logs = [];

    public IReadOnlyList<string> Logs => _logs;

    public IDisposable BeginScope<TState>(TState state) where TState : notnull
    {
        return NullScope.Instance;
    }

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        ArgumentNullException.ThrowIfNull(formatter, nameof(formatter));
        var message = formatter(state, exception);
        _logs.Add($"[{logLevel}] {message}");
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    private class NullScope : IDisposable
    {
        public static readonly NullScope Instance = new();

        public void Dispose() { }
    }
}
