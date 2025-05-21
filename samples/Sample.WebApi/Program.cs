//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Sample.WebApi.Endpoints;
using D20Tek.LowDb;
using D20Tek.MinimalApi.DevView;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();

builder.Services.AddLowDbAsync<TasksDocument>(b =>
    b.UseFileDatabase("tasks.json")
     .WithFolder("data")
     .WithLifetime(ServiceLifetime.Scoped));
builder.Services.AddScoped<ITasksRepository, TasksRepository>();
builder.Services.AddDevView(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDevView();
}

app.UseHttpsRedirection();

//app.MapTaskEntityEndpoints();
app.MapTaskV2Endpoints();

app.Run();
