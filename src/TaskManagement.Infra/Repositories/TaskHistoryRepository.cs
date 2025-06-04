using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Infra.Context;

namespace TaskManagement.Infra.Repositories
{
    public sealed class TaskHistoryRepository(TaskManagementDbContext dbContext)
        : Repository<TaskHistory>(dbContext),
            ITaskHistoryRepository { }
}
