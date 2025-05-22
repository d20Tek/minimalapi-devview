using D20Tek.LowDb;
using D20Tek.MinimalApi.DevView;
using Sample.WebApi.Endpoints.Forecasts;
using Microsoft.AspNetCore.Mvc;
using Sample.WebApi.Endpoints.Tasks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();

builder.Services.AddLowDbAsync<TasksDocument>(b =>
    b.UseFileDatabase("tasks.json")
     .WithFolder("data")
     .WithLifetime(ServiceLifetime.Scoped));
builder.Services.AddDevView(builder.Configuration);

builder.Services.AddScoped<ITasksRepository, TasksRepository>()
                .AddScoped<GetForecastsHandler>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDevView();
}

app.UseHttpsRedirection();

app.MapTaskEntityEndpoints();
app.MapTaskV2Endpoints();

app.MapGet("/weatherforecast", ([FromServices] GetForecastsHandler handler) => handler.Handle())
   .Produces<ForecastResponse[]>(StatusCodes.Status200OK)
   .WithName("GetWeatherForecast");

app.Run();
