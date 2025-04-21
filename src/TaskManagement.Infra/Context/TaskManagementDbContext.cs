using Microsoft.EntityFrameworkCore;
using TaskManagement.CrossCutting.Persistences;
using TaskManagement.Domain.Entities;
using TaskManagement.Infra.Mappers;
using Task = TaskManagement.Domain.Entities.Task;

namespace TaskManagement.Infra.Context
{
    public sealed class TaskManagementDbContext(DbContextOptions<TaskManagementDbContext> options) :
        DbContext(options),
        IUnitOfWork
    {
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<User> Users { get; set; }

        public async Task<int> CommitAsync(CancellationToken cancellationToken = default) =>
            await SaveChangesAsync(cancellationToken);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new TaskEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectEntityTypeConfiguration());
            //modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new TaskHistoryEntityTypeConfiguration());
        }
    }
}
