using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Security.Claims;
using TaskManagement.CrossCutting.Persistences;
using TaskManagement.Domain.Entities;
using TaskManagement.Infra.Mappers;
using Task = TaskManagement.Domain.Entities.Task;

namespace TaskManagement.Infra.Context
{
    public sealed class TaskManagementDbContext(DbContextOptions<TaskManagementDbContext> options, 
        IHttpContextAccessor contextAccessor) : DbContext(options), IUnitOfWork
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
            modelBuilder.ApplyConfiguration(new TaskHistoryEntityTypeConfiguration());
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var userId = GetUserId();

            var utcNow = DateTime.UtcNow;
            var entities = ChangeTracker.Entries<Entity>().ToList();

            foreach (var entry in entities)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        SetCurrentPropertyValue(entry, nameof(Entity.CreatedAt), utcNow);
                        SetCurrentPropertyValue(entry, nameof(Entity.CreatedBy), userId);
                        break;
                    case EntityState.Modified:
                        SetCurrentPropertyValue(entry, nameof(Entity.UpdatedAt), utcNow);
                        SetCurrentPropertyValue(entry, nameof(Entity.UpdatedBy), userId);
                        break;
                    case EntityState.Deleted:
                        SetCurrentPropertyValue(entry, nameof(Entity.DeletedAt), utcNow);
                        SetCurrentPropertyValue(entry, nameof(Entity.DeletedBy), userId);
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        private Guid GetUserId()
        {
            var userIdValue = contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? Guid.Empty.ToString();
            return Guid.TryParse(userIdValue, out var userId) ? userId : Guid.Empty;
        }

        private static void SetCurrentPropertyValue(
            EntityEntry entry,
            string propertyName,
            object? value) => entry.Property(propertyName).CurrentValue = value;
    }
}
