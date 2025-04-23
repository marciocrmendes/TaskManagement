using MediatR;
using TaskManagement.CrossCutting.Dtos.Task;
using TaskManagement.Domain.Aggregates.Task.Queries;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Domain.Aggregates.Task.Handlers
{
    public sealed class GetTaskByIdQueryHandler(ITaskRepository taskRepository) :
        IRequestHandler<GetTaskByIdQuery, TaskResponse>
    {
        public async Task<TaskResponse> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await taskRepository.FindAsync(request.Id, cancellationToken);
            if (result is null) return default!;

            return result.ToResponse();
        }
    }
}
