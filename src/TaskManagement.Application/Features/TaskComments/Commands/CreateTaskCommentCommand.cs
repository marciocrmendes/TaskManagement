using System.Text.Json.Serialization;
using MediatR;

namespace TaskManagement.Application.Features.TaskComments.Commands
{
    public sealed class CreateTaskCommentCommand : IRequest<Unit>
    {
        [JsonIgnore]
        public Guid TaskId { get; set; }
        public string Comment { get; set; } = string.Empty;
    }
}
