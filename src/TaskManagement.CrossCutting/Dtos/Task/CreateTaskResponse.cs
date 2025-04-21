using TaskManagement.CrossCutting.Enums;

namespace TaskManagement.CrossCutting.Dtos.Task
{
    public record CreateTaskResponse(Guid Id,
        Guid ProjectId,
        string Title,
        DateTime? DueDate,
        string? Description,
        TaskPriorityEnum Priority);
}
