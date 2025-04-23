using MediatR;
using TaskManagement.CrossCutting.Dtos;
using TaskManagement.CrossCutting.Dtos.Task;

namespace TaskManagement.Domain.Aggregates.Project.Queries
{
    public record GetTasksByIdQuery(Guid ProjectId) : IRequest<GenericResponseList<TaskResponse>>;
}
