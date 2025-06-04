using MediatR;
using TaskManagement.Application.Common;
using TaskManagement.Application.Features.Tasks.Commands;
using TaskManagement.CrossCutting.Dtos.Task;
using TaskManagement.CrossCutting.Notifications;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Application.Features.Tasks.Handlers
{
    public sealed class CreateTaskHandler(
        ITaskRepository taskRepository,
        IProjectRepository projectRepository,
        INotificationHandler notificationHandler
    )
        : CommandHandler(taskRepository.UnitOfWork),
            IRequestHandler<CreateTaskCommand, CreateTaskResponse>
    {
        public async Task<CreateTaskResponse> Handle(
            CreateTaskCommand request,
            CancellationToken cancellationToken
        )
        {
            var projectExists = await projectRepository.ExistsAsync(
                x => x.Id == request.ProjectId,
                cancellationToken
            );
            if (!projectExists)
            {
                notificationHandler.AddNotification(Domain.Entities.Project.Errors.NotFound);
                return default!;
            }

            var taskCount = await projectRepository.GetTasksCountByIdAsync(request.ProjectId);
            if (taskCount == 20)
            {
                notificationHandler.AddNotification(Domain.Entities.Project.Errors.TaskLenghtLimit);
                return default!;
            }

            var task = new Domain.Entities.Task(
                request.ProjectId,
                request.Title,
                request.Description,
                request.DueDate,
                request.Status,
                request.Priority
            );

            await taskRepository.AddAsync(task, cancellationToken);

            await CommitAsync(cancellationToken);

            return new(
                task.Id,
                task.ProjectId,
                task.Title,
                task.DueDate,
                task.Description,
                task.Priority
            );
        }
    }
}
