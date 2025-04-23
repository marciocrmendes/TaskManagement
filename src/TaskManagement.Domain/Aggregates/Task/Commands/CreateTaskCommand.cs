using MediatR;
using TaskManagement.CrossCutting.Dtos.Task;
using TaskManagement.CrossCutting.Enums;

namespace TaskManagement.Domain.Aggregates.Task.Commands
{
    public record CreateTaskCommand(Guid ProjectId,
        string Title,
        string Description,
        DateTime? DueDate,
        TaskStatusEnum Status,
        TaskPriorityEnum Priority) : IRequest<CreateTaskResponse>;
}
