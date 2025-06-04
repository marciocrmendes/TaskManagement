using TaskManagement.CrossCutting.Notifications;

namespace TaskManagement.Domain.Entities
{
    public class Project : Entity<Guid>
    {
        public Project(string name, string? description)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
        }

        public Project(Guid id, string name, string? description)
        {
            Id = id;
            Name = name;
            Description = description;
        }

        public string Name { get; private set; }
        public string? Description { get; private set; }

        public virtual ICollection<Task> Tasks { get; private set; } = [];

        public static class Errors
        {
            public static readonly Notification NotFound = new(
                "ProjectNotFound",
                "Projeto não encontrado"
            );
            public static readonly Notification HasPendingTask = new(
                "ProjectHasPendingTask",
                """                
                Não é possível excluir o projeto, pois existem tarefas pendentes. Conclua ou remova as tarefas pendentes                
                """
            );
            public static readonly Notification TaskLenghtLimit = new(
                "TaskLenghtLimit",
                "O projeto não pode ter mais de 20 tarefas."
            );
        }
    }
}
