using MediatR;
using System.Text.Json.Serialization;

namespace TaskManagement.Domain.Aggregates.TaskComment.Commands
{
    public sealed class CreateTaskCommentCommand : IRequest<Unit>
    {
        [JsonIgnore]
        public Guid TaskId { get; set; }
        public string Comment { get; set; } = string.Empty;
    }
}
