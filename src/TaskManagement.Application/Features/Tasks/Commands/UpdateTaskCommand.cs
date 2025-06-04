using MediatR;
using TaskManagement.CrossCutting.Dtos.Task;
using TaskManagement.CrossCutting.Enums;

namespace TaskManagement.Application.Features.Tasks.Commands
{
    public record UpdateTaskCommand(
        Guid Id,
        Guid ProjectId,
        string Title,
        string Description,
        DateTime? DueDate,
        TaskStatusEnum Status
    ) : IRequest<TaskResponse>;
}
