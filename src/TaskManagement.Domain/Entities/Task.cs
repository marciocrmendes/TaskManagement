using TaskManagement.CrossCutting.Enums;

namespace TaskManagement.Domain.Entities
{
    public class Task(Guid projectId,
        string title,
        string? description,
        DateTime? dueDate,
        TaskPriorityEnum priority) : Entity<Guid>
    {
        public Guid ProjectId { get; private set; } = projectId;
        public string Title { get; private set; } = title;
        public string? Description { get; private set; } = description;
        public DateTime? DueDate { get; private set; } = dueDate;
        public TaskStatusEnum Status { get; private set; } = TaskStatusEnum.Pending;
        public TaskPriorityEnum Priority { get; init; } = priority;

        public virtual Project Project { get; private set; } = null!;
        public virtual ICollection<TaskHistory> HistoryList { get; private set; } = [];
        public virtual ICollection<TaskComment> Comments { get; private set; } = [];

        public Task ChangeStatus(TaskStatusEnum status)
        {
            Status = status;
            return this;
        }
    }
}
