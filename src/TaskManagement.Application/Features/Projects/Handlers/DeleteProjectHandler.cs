using MediatR;
using TaskManagement.Application.Common;
using TaskManagement.Application.Features.Projects.Commands;
using TaskManagement.CrossCutting.Notifications;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Application.Features.Projects.Handlers
{
    public sealed class DeleteProjectHandler(
        IProjectRepository projectRepository,
        ITaskRepository taskRepository,
        INotificationHandler notificationHandler
    ) : CommandHandler(projectRepository.UnitOfWork), IRequestHandler<DeleteProjectCommand, Unit>
    {
        public async Task<Unit> Handle(
            DeleteProjectCommand request,
            CancellationToken cancellationToken
        )
        {
            var project = await projectRepository.FindAsync(request.Id, cancellationToken);
            if (project is null)
            {
                notificationHandler.AddNotification(Domain.Entities.Project.Errors.NotFound);
                return default!;
            }

            var hasPendingTask = await taskRepository.ExistsAsync(
                x => x.Id == request.Id && x.Status == CrossCutting.Enums.TaskStatusEnum.Pending,
                cancellationToken
            );

            if (hasPendingTask)
            {
                notificationHandler.AddNotification(Domain.Entities.Project.Errors.HasPendingTask);
                return default!;
            }

            await projectRepository.DeleteAsync(project);

            await CommitAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
