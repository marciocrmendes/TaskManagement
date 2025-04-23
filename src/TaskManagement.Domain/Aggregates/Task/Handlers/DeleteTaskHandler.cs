using MediatR;
using TaskManagement.CrossCutting.Notifications;
using TaskManagement.Domain.Aggregates.Task.Commands;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Domain.Aggregates.Task.Handlers
{
    public sealed class DeleteTaskHandler(ITaskRepository taskRepository,
        INotificationHandler notificationHandler) :
        CommandHandler(taskRepository.UnitOfWork),
        IRequestHandler<DeleteTaskCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await taskRepository.FindAsync(request.Id, cancellationToken);
            if (task == null)
            {
                notificationHandler.AddNotification(Entities.Task.Errors.NotFound);
                return default!;
            }

            await taskRepository.DeleteAsync(task);

            await CommitAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
