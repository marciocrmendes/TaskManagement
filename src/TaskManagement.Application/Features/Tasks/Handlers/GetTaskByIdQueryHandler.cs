using MediatR;
using TaskManagement.Application.Common.Extensions;
using TaskManagement.Application.Features.Tasks.Queries;
using TaskManagement.CrossCutting.Dtos.Task;
using TaskManagement.Domain.Interfaces;
using DomainTask = TaskManagement.Domain.Entities.Task;

namespace TaskManagement.Application.Features.Tasks.Handlers
{
    public sealed class GetTaskByIdQueryHandler(ITaskRepository taskRepository)
        : IRequestHandler<GetTaskByIdQuery, TaskResponse>
    {
        public async System.Threading.Tasks.Task<TaskResponse> Handle(
            GetTaskByIdQuery request,
            CancellationToken cancellationToken
        )
        {
            var result = await taskRepository.FindAsync(request.Id, cancellationToken);
            if (result is null)
                return default!;

            return result.ToResponse();
        }
    }
}
