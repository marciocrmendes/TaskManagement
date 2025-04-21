using TaskManagement.Domain.Interfaces;
using TaskManagement.Infra.Context;
using Task = TaskManagement.Domain.Entities.Task;

namespace TaskManagement.Infra.Repositories
{
    public sealed class TaskRepository(TaskManagementDbContext dbContext) : Repository<Task>(dbContext), ITaskRepository
    {
    }
}
