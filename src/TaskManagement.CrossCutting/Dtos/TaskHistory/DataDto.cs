namespace TaskManagement.CrossCutting.Dtos.TaskHistory
{
    public sealed record ChangeTaskHistoryDto(
        string PropertyName,
        string? OldValue,
        string? NewValue
    );
}
