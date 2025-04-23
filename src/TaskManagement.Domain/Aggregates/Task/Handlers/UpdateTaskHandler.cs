using MediatR;
using TaskManagement.CrossCutting.Dtos.Task;
using TaskManagement.CrossCutting.Notifications;
using TaskManagement.Domain.Aggregates.Task.Commands;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Domain.Aggregates.Task.Handlers
{
    public sealed class UpdateTaskHandler(ITaskRepository taskRepository,
        IProjectRepository projectRepository,
        INotificationHandler notificationHandler) :
        CommandHandler(taskRepository.UnitOfWork),
        IRequestHandler<UpdateTaskCommand, TaskResponse>
    {
        public async Task<TaskResponse> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await taskRepository.GetByIdAsync(request.Id, cancellationToken);
            if (task == null)
            {
                notificationHandler.AddNotification(Entities.Task.Errors.NotFound);
                return default!;
            }

            var exists = await projectRepository
                .ExistsAsync(x => x.Id == request.ProjectId, cancellationToken);
            if (!exists)
            {
                notificationHandler.AddNotification(Entities.Project.Errors.NotFound);
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
