using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagement.CrossCutting.Enums;
using Task = TaskManagement.Domain.Entities.Task;

namespace TaskManagement.Infra.Mappers
{
    internal sealed class TaskEntityTypeConfiguration : IEntityTypeConfiguration<Task>
    {
        public void Configure(EntityTypeBuilder<Task> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id);

            builder.Property(e => e.Title).HasMaxLength(100);

            builder.Property(e => e.Description).HasMaxLength(3000);

            builder.Property(e => e.DueDate);

            builder
                .Property(e => e.Status)
                .HasDefaultValue(TaskStatusEnum.Pending)
                .HasConversion(v => (int)v, v => Enum.Parse<TaskStatusEnum>(v.ToString()));
            ;

            builder
                .Property(e => e.Priority)
                .HasDefaultValue(TaskPriorityEnum.Low)
                .HasConversion(v => (int)v, v => Enum.Parse<TaskPriorityEnum>(v.ToString()));

            builder.Property(e => e.CreatedAt).HasDefaultValueSql("now()");

            builder.HasMany(t => t.HistoryList).WithOne(h => h.Task).HasForeignKey(h => h.TaskId);

            //builder.HasMany(t => t.Comments)
            //    .WithOne(c => c.Task)
            //    .HasForeignKey(c => c.TaskId);
        }
    }
}
