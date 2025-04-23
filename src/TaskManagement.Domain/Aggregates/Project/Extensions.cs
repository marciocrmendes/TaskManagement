using TaskManagement.CrossCutting.Dtos.Project;

namespace TaskManagement.Domain.Aggregates.Project
{
    public static class Extensions
    {
        public static Entities.Project ToEntity(this ProjectResponse response) =>
            new(response.Id, response.Name, response.Description);

        public static ProjectResponse ToResponse(this Entities.Project project) =>
            new(project.Id, project.Name, project.Description);
    }
}
