namespace Sample.WebApi.Endpoints.Tasks;

internal record CreateTaskRequest(string Name);

internal record UpdateTaskRequest(string Name, bool IsCompleted);
