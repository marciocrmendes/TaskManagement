using MediatR;
using TaskManagement.CrossCutting.Dtos.Task;

namespace TaskManagement.Application.Features.Tasks.Queries
{
    public record GetTaskByIdQuery(Guid Id) : IRequest<TaskResponse>;
}
