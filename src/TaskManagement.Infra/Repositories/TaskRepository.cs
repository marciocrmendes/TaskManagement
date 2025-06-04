using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Infra.Context;
using Task = TaskManagement.Domain.Entities.Task;

namespace TaskManagement.Infra.Repositories
{
    public sealed class TaskRepository(TaskManagementDbContext dbContext)
        : Repository<Task>(dbContext),
            ITaskRepository
    {
        public async Task<Task?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default
        ) => await dbContext.Tasks.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
}
