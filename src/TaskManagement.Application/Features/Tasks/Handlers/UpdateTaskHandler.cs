using MediatR;
using TaskManagement.Application.Common;
using TaskManagement.Application.Common.Extensions;
using TaskManagement.Application.Features.Tasks.Commands;
using TaskManagement.CrossCutting.Dtos.Task;
using TaskManagement.CrossCutting.Notifications;
using TaskManagement.Domain.Interfaces;
using DomainTask = TaskManagement.Domain.Entities.Task;

namespace TaskManagement.Application.Features.Tasks.Handlers
{
    public sealed class UpdateTaskHandler(
        ITaskRepository taskRepository,
        IProjectRepository projectRepository,
        INotificationHandler notificationHandler
    ) : CommandHandler(taskRepository.UnitOfWork), IRequestHandler<UpdateTaskCommand, TaskResponse>
    {
        public async System.Threading.Tasks.Task<TaskResponse> Handle(
            UpdateTaskCommand request,
            CancellationToken cancellationToken
        )
        {
            var task = await taskRepository.GetByIdAsync(request.Id, cancellationToken);
            if (task == null)
            {
                notificationHandler.AddNotification(DomainTask.Errors.NotFound);
                return default!;
            }

            var exists = await projectRepository.ExistsAsync(
                x => x.Id == request.ProjectId,
                cancellationToken
            );
            if (!exists)
            {
                notificationHandler.AddNotification(Domain.Entities.Project.Errors.NotFound);
                return default!;
            }

            task.ChangeTitle(request.Title)
                .ChangeDescription(request.Description)
                .ChangeDueDate(request.DueDate)
                .ChangeStatus(request.Status);

            await taskRepository.UpdateAsync(task);

            await CommitAsync(cancellationToken);

            return task.ToResponse();
        }
    }
}
