namespace Sample.WebApi.Endpoints.Tasks;

internal class TaskEntity
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public bool IsCompleted { get; set; }
}
