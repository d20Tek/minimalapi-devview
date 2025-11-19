using D20Tek.LowDb;
using D20Tek.LowDb.Repositories;

namespace Sample.WebApi.Endpoints.Tasks;

internal interface ITasksRepository : IRepositoryAsync<TaskEntity>;

internal class TasksRepository(LowDbAsync<TasksDocument> db) :
    LowDbAsyncRepository<TaskEntity, TasksDocument>(db, x => x.Tasks), ITasksRepository
{
}
