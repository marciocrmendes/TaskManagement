using TaskManagement.CrossCutting.Dtos.Task;

namespace TaskManagement.Domain.Aggregates.Task
{
    public static class Extensions
    {
        public static Entities.Task ToEntity(this TaskResponse response) =>
            new(response.Id, 
                response.ProjectId,
                response.Title, 
                response.Description,
                response.DueDate,
                response.Status,
                response.Priority);

        public static TaskResponse ToResponse(this Entities.Task project) =>
            new(project.Id,
                project.ProjectId,
                project.Title,
                project.Description,
                project.DueDate,
                project.Status,
                project.Priority);
    }
}
