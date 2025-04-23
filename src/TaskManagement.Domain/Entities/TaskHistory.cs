namespace TaskManagement.Domain.Entities
{
    public class TaskHistory(Guid taskId,
        string @event,
        string data,
        Guid modifiedBy) : Entity<Guid>
    {
        public Guid TaskId { get; set; } = taskId;
        public DateTime Date { get; private set; } = DateTime.UtcNow;
        public string Event { get; private set; } = @event;
        public string Data { get; private set; } = data;
        public Guid ModifiedBy { get; private set; } = modifiedBy;

        public Task Task { get; set; } = null!;
    }
}
