using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Infra.Context;

namespace TaskManagement.Infra.Repositories
{
    public sealed class ProjectRepository(TaskManagementDbContext dbContext) : Repository<Project>(dbContext), IProjectRepository
    {
        public async Task<int?> GetTasksCountByIdAsync(Guid id)
        {
            var tasksCount = await dbContext.Projects
                .AsNoTracking()
                .Include(p => p.Tasks)
                .Select(x => new
                {
                    x.Id,
                    Count = x.Tasks.Select(x => x.Id).Count(),
                })
                .FirstOrDefaultAsync(x => x.Id == id);

            return tasksCount?.Count ?? 0;
        }
    }
}
