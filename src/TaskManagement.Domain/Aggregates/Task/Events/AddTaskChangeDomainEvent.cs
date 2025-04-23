using TaskManagement.CrossCutting.Events;

namespace TaskManagement.Domain.Aggregates.Task.Events
{
    public sealed record AddTaskChangeDomainEvent(Guid TaskId, string Data) : IDomainEvent;
}
