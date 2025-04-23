using TaskManagement.CrossCutting.Enums;

namespace TaskManagement.CrossCutting.Dtos.Task
{
    public sealed record TaskResponse(Guid Id, 
        Guid ProjectId, 
        string Title, 
        string? Description, 
        DateTime? DueDate,
        TaskStatusEnum Status,
        TaskPriorityEnum Priority);
}
