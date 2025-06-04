using MediatR;
using TaskManagement.Application.Common;
using TaskManagement.Application.Features.Tasks.Commands;
using TaskManagement.CrossCutting.Notifications;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Application.Features.Tasks.Handlers
{
    public sealed class DeleteTaskHandler(
        ITaskRepository taskRepository,
        INotificationHandler notificationHandler
    ) : CommandHandler(taskRepository.UnitOfWork), IRequestHandler<DeleteTaskCommand, Unit>
    {
        public async Task<Unit> Handle(
            DeleteTaskCommand request,
            CancellationToken cancellationToken
        )
        {
            var task = await taskRepository.FindAsync(request.Id, cancellationToken);
            if (task == null)
            {
                notificationHandler.AddNotification(Domain.Entities.Task.Errors.NotFound);
                return default!;
            }

            await taskRepository.DeleteAsync(task);

            await CommitAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
