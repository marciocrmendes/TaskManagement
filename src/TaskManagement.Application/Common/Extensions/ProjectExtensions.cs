using TaskManagement.CrossCutting.Dtos.Project;
using DomainProject = TaskManagement.Domain.Entities.Project;

namespace TaskManagement.Application.Common.Extensions
{
    public static class ProjectExtensions
    {
        public static DomainProject ToEntity(this ProjectResponse response) =>
            new(response.Id, response.Name, response.Description);

        public static ProjectResponse ToResponse(this DomainProject project) =>
            new(project.Id, project.Name, project.Description);
    }
}
