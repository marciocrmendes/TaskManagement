namespace TaskManagement.Domain.Entities
{
    public class User : Entity<Guid>
    {
        private User() { }

        public required string Name { get; set; }
        public required string Email { get; set; }

        public virtual ICollection<Project> Projects { get; set; } = [];

        public User Create(string name, string email, ICollection<Project> projects)
        {
            Name = name;
            Email = email;
            Projects = projects;
            return this;
        }
    }
}
