using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Infra.Context;

namespace TaskManagement.Infra.Repositories
{
    public sealed class TaskCommentRepository(TaskManagementDbContext dbContext) : Repository<TaskComment>(dbContext),
        ITaskCommentRepository
    {
    }
}
