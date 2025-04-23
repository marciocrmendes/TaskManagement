using MediatR;
using TaskManagement.CrossCutting.Dtos.Task;

namespace TaskManagement.Domain.Aggregates.Task.Queries
{
    public record GetTaskByIdQuery(Guid Id) : IRequest<TaskResponse>;
}
