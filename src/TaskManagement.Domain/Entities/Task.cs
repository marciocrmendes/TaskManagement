using System.Text.Json;
using TaskManagement.CrossCutting.Dtos.TaskHistory;
using TaskManagement.CrossCutting.Enums;
using TaskManagement.CrossCutting.Notifications;
using TaskManagement.Domain.Aggregates.Task.Events;

namespace TaskManagement.Domain.Entities
{
    public class Task(
        Guid projectId,
        string title,
        string? description,
        DateTime? dueDate,
        TaskStatusEnum status,
        TaskPriorityEnum priority
    ) : Entity<Guid>
    {
        public Task(
            Guid id,
            Guid projectId,
            string title,
            string? description,
            DateTime? dueDate,
            TaskStatusEnum status,
            TaskPriorityEnum priority
        )
            : this(projectId, title, description, dueDate, status, priority)
        {
            Id = id;
            Status = status;
        }

        public Guid ProjectId { get; private set; } = projectId;
        public string Title { get; private set; } = title;
        public string? Description { get; private set; } = description;
        public DateTime? DueDate { get; private set; } = dueDate;
        public TaskStatusEnum Status { get; private set; } = status;
        public TaskPriorityEnum Priority { get; init; } = priority;

        public virtual Project Project { get; private set; } = null!;
        public virtual ICollection<TaskHistory> HistoryList { get; private set; } = [];

        //public virtual ICollection<TaskComment> Comments { get; private set; } = [];

        public Task ChangeTitle(string title)
        {
            if (Title != title)
            {
                var data = new ChangeTaskHistoryDto(nameof(Title), Title, title);

                RaiseDomainEvent(
                    new AddTaskChangeDomainEvent(TaskId: Id, Data: JsonSerializer.Serialize(data))
                );
            }

            Title = title;

            return this;
        }

        public Task ChangeDescription(string? description)
        {
            if (Description != description)
            {
                var data = new ChangeTaskHistoryDto(nameof(Description), Description, description);

                RaiseDomainEvent(
                    new AddTaskChangeDomainEvent(TaskId: Id, Data: JsonSerializer.Serialize(data))
                );
            }

            Description = description;
            return this;
        }

        public Task ChangeDueDate(DateTime? dueDate)
        {
            if (DueDate != dueDate)
            {
                var data = new ChangeTaskHistoryDto(
                    nameof(DueDate),
                    DueDate?.ToShortDateString(),
                    dueDate?.ToShortDateString()
                );

                RaiseDomainEvent(
                    new AddTaskChangeDomainEvent(TaskId: Id, Data: JsonSerializer.Serialize(data))
                );
            }
            DueDate = dueDate;
            return this;
        }

        public Task ChangeStatus(TaskStatusEnum status)
        {
            if (Status != status)
            {
                var data = new ChangeTaskHistoryDto(
                    nameof(Status),
                    Status.ToString(),
                    status.ToString()
                );

                RaiseDomainEvent(
                    new AddTaskChangeDomainEvent(TaskId: Id, Data: JsonSerializer.Serialize(data))
                );
            }
            Status = status;
            return this;
        }

        public static class Errors
        {
            public static readonly Notification NotFound = new(
                "TaskNotFound",
                "Tarefa não encontrada"
            );
        }
    }
}
