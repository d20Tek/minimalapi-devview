using D20Tek.LowDb.Repositories;

namespace Sample.WebApi.Endpoints.Tasks;

internal class TasksDocument : DbDocument
{
    public int LastId { get; set; } = 0;

    public string Version { get; set; } = "1.0";

    public HashSet<TaskEntity> Tasks { get; init; } = [];

    public int GetNextId() => ++LastId;
}
