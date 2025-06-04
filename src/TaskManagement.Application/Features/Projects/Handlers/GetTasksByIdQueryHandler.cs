using MediatR;
using TaskManagement.Application.Common.Extensions;
using TaskManagement.Application.Features.Projects.Queries;
using TaskManagement.CrossCutting.Dtos;
using TaskManagement.CrossCutting.Dtos.Task;
using TaskManagement.Domain.Interfaces;
using DomainTask = TaskManagement.Domain.Entities.Task;

namespace TaskManagement.Application.Features.Projects.Handlers
{
    public sealed class GetTasksByIdQueryHandler(ITaskRepository taskRepository)
        : IRequestHandler<GetTasksByIdQuery, GenericResponseList<TaskResponse>>
    {
        public async System.Threading.Tasks.Task<GenericResponseList<TaskResponse>> Handle(
            GetTasksByIdQuery request,
            CancellationToken cancellationToken
        )
        {
            var result = await taskRepository.WhereAsync(
                x => x.ProjectId == request.ProjectId,
                cancellationToken
            );
            return new([.. result.Select(x => x.ToResponse())]);
        }
    }
}
