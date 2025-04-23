using TaskManagement.CrossCutting.Events;

namespace TaskManagement.Domain.Aggregates.TaskComment.Events
{
    public sealed record AddTaskCommentDomainEvent(string Comment) : IDomainEvent;
}
