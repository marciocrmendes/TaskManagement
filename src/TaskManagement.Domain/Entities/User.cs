namespace TaskManagement.Domain.Entities
{
    public class User : Entity<Guid>
    {
        private User() { }

        public User(string name, string email)
        {
            Name = name;
            Email = email;
        }

        public string Name { get; private set; }
        public string Email { get; private set; }

        public virtual ICollection<Project> Projects { get; set; } = [];
    }
}
