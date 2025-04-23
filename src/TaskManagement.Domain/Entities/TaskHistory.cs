namespace TaskManagement.Domain.Entities
{
    public class TaskHistory : Entity<Guid>
    {
        public Guid TaskId { get; set; }
        public DateTime Date { get; private set; } = DateTime.UtcNow;
        public string Event { get; private set; }
        public string Data { get; private set; }
        public Guid ModifiedBy { get; private set; }

        public Task Task { get; set; } = null!;

        public TaskHistory(Guid taskId,
            string @event,
            string data,
            Guid modifiedBy)
        {
            TaskId = taskId;
            Event = @event;
            Data = data;
            ModifiedBy = modifiedBy;
        }
    }
}
