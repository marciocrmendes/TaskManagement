using MediatR;
using TaskManagement.CrossCutting.Dtos;
using TaskManagement.CrossCutting.Dtos.Task;
using TaskManagement.Domain.Aggregates.Project.Queries;
using TaskManagement.Domain.Aggregates.Task;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Domain.Aggregates.Project.Handlers
{
    public sealed class GetTasksByIdQueryHandler(ITaskRepository taskRepository) : 
        IRequestHandler<GetTasksByIdQuery, GenericResponseList<TaskResponse>>
    {
        public async Task<GenericResponseList<TaskResponse>> Handle(GetTasksByIdQuery request, 
            CancellationToken cancellationToken)
        {
            var result = await taskRepository.WhereAsync(x => x.ProjectId == request.ProjectId, cancellationToken);
            return new([.. result.Select(x => x.ToResponse())]);
        }
    }
}
