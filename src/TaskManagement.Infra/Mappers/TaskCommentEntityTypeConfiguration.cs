using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Infra.Mappers
{
    internal sealed class TaskCommentEntityTypeConfiguration : IEntityTypeConfiguration<TaskComment>
    {
        public void Configure(EntityTypeBuilder<TaskComment> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id);

            builder.Property(e => e.Date);

            builder.Property(e => e.Comment)
                .IsRequired()
                .HasMaxLength(3000);

            builder.Property(e => e.CommentedBy);

            builder.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()");
        }
    }
}
