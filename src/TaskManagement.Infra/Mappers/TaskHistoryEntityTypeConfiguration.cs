using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Infra.Mappers
{
    internal sealed class TaskHistoryEntityTypeConfiguration : IEntityTypeConfiguration<TaskHistory>
    {
        public void Configure(EntityTypeBuilder<TaskHistory> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id);

            builder.Property(e => e.Event);

            builder.Property(e => e.Data).HasColumnType("jsonb");

            builder.Property(e => e.Date);

            builder.Property(e => e.ModifiedBy);

            builder.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
        }
    }
}
