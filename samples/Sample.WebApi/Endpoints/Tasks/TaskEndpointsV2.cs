﻿using D20Tek.Functional.AspNetCore.MinimalApi;
using D20Tek.Functional.Async;
using D20Tek.LowDb;

namespace Sample.WebApi.Endpoints.Tasks;

public static class TaskEndpointsV2
{
    public static void MapTaskV2Endpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/v2/tasks")
                          .WithTags("Tasks V2");

        group.MapGet("/", GetAllTasks)
             .Produces<TaskEntity[]>(StatusCodes.Status200OK)
             .WithName("GetAllTasks.V2")
             .WithOpenApi();

        group.MapGet("/{id}", async (int id, ITasksRepository repo) =>
        {
            var result = await repo.GetByIdAsync(t => t.Id, id);
            return result.ToApiResult();
        })
        .Produces<TaskEntity[]>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithName("GetTaskById.V2")
        .WithOpenApi();

        group.MapPost("/", async (CreateTaskRequest request, LowDbAsync<TasksDocument> db, ITasksRepository repo) =>
        {
            var entity = await CreateTaskEntity(request, db);
            var result = await repo.AddAsync(entity)
                                   .IterAsync(_ => repo.SaveChangesAsync());

            return result.Match(
                response => TypedResults.Created($"/api/v1/tasks/{response?.Id}", response),
                errors => TypedResults.Extensions.Problem(errors));
        })
        .Produces<TaskEntity>(StatusCodes.Status201Created)
        .ProducesValidationProblem(StatusCodes.Status400BadRequest)
        .WithName("CreateTask.V2")
        .WithOpenApi();

        group.MapPut("/{id}", async (int id, UpdateTaskRequest request, ITasksRepository repo) =>
        {
            var result = await repo.GetByIdAsync(t => t.Id, id)
                                   .MapAsync(prev => ChangeTaskEntity(prev, request))
                                   .BindAsync(updated => repo.UpdateAsync(updated))
                                   .IterAsync(_ => repo.SaveChangesAsync());

            return result.ToApiResult();
        })
        .Produces<TaskEntity>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesValidationProblem(StatusCodes.Status400BadRequest)
        .WithName("UpdateTask.V2")
        .WithOpenApi();

        group.MapDelete("/{id}", async (int id, ITasksRepository repo) =>
        {
            var result = await repo.GetByIdAsync(t => t.Id, id)
                                   .BindAsync(task => repo.RemoveAsync(task))
                                   .IterAsync(_ => repo.SaveChangesAsync());

            return result.ToApiResult();
        })
        .Produces<TaskEntity>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesValidationProblem(StatusCodes.Status400BadRequest)
        .WithName("DeleteTask.V2")
        .WithOpenApi();
    }

    private async static Task<IResult> GetAllTasks(ITasksRepository repo)
    {
        var result = await repo.GetAllAsync();
        return result.ToApiResult();
    }

    private static async Task<TaskEntity> CreateTaskEntity(CreateTaskRequest request, LowDbAsync<TasksDocument> db)
    {
        var doc = await db.Get();
        return new TaskEntity
        {
            Id = doc.GetNextId(),
            Name = request.Name,
            IsCompleted = false
        };
    }

    private static Task<TaskEntity> ChangeTaskEntity(TaskEntity task, UpdateTaskRequest updates)
    {
        task.Name = updates.Name;
        task.IsCompleted = updates.IsCompleted;

        return Task.FromResult(task);
    }
}
