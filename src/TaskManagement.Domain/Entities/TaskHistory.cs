namespace TaskManagement.Domain.Entities
{
    public class TaskHistory(Guid taskId,
        DateTime date,
        string propertyName,
        object oldValue,
        object newValue,
        Guid modifiedBy) : Entity<Guid>
    {
        public Guid TaskId { get; set; } = taskId;
        public DateTime Date { get; private set; } = date;
        public string PropertyName { get; private set; } = propertyName;
        public object OldValue { get; private set; } = oldValue;
        public object NewValue { get; private set; } = newValue;
        public Guid ModifiedBy { get; private set; } = modifiedBy;

        public Task Task { get; set; } = null!;
    }
}
