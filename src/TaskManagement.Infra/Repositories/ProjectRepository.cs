using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Infra.Context;

namespace TaskManagement.Infra.Repositories
{
    public sealed class ProjectRepository(TaskManagementDbContext dbContext) : Repository<Project>(dbContext), IProjectRepository
    {
    }
}
