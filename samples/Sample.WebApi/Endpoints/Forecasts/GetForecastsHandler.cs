namespace Sample.WebApi.Endpoints.Forecasts;

public sealed class GetForecastsHandler
{
    private static readonly string[] _summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    public ForecastResponse[] Handle()
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
            new ForecastResponse
            (
                DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Random.Shared.Next(-20, 55),
                _summaries[Random.Shared.Next(_summaries.Length)]
            ))
            .ToArray();
        return forecast;
    }
}
