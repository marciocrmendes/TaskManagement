using TaskManagement.CrossCutting.Dtos.Task;
using DomainTask = TaskManagement.Domain.Entities.Task;

namespace TaskManagement.Application.Common.Extensions
{
    public static class TaskExtensions
    {
        public static DomainTask ToEntity(this TaskResponse response) =>
            new(
                response.Id,
                response.ProjectId,
                response.Title,
                response.Description,
                response.DueDate,
                response.Status,
                response.Priority
            );

        public static TaskResponse ToResponse(this DomainTask task) =>
            new(
                task.Id,
                task.ProjectId,
                task.Title,
                task.Description,
                task.DueDate,
                task.Status,
                task.Priority
            );
    }
}
