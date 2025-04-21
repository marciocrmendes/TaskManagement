namespace TaskManagement.Domain.Entities
{
    public class TaskComment(Guid taskId,
        DateTime date,
        string comment,
        Guid userId) : Entity<Guid>
    {
        public Guid TaskId { get; private set; } = taskId;
        public DateTime Date { get; private set; } = date;
        public string Comment { get; private set; } = comment;
        public Guid UserId { get; private set; } = userId;
        public Guid CommentedBy { get; private set; } = userId;

        public Task Task { get; set; } = null!;
    }
}
