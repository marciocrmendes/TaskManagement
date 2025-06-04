using MediatR;
using TaskManagement.CrossCutting.Dtos;
using TaskManagement.CrossCutting.Dtos.Task;

namespace TaskManagement.Application.Features.Projects.Queries
{
    public record GetTasksByIdQuery(Guid ProjectId) : IRequest<GenericResponseList<TaskResponse>>;
}
