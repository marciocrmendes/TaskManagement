using TaskManagement.CrossCutting.Notifications;

namespace TaskManagement.Domain.Entities
{
    public class Project(string name, string? description) : Entity<Guid>
    {
        public string Name { get; private set; } = name;
        public string? Description { get; private set; } = description;

        public virtual ICollection<Task> Tasks { get; private set; } = [];

        public static class Errors
        {
            public static readonly Notification ProjectNotFound = new("ProjectNotFound", "Projeto não encontrado");
        }
    }
}
