using MediatR;
using TaskManagement.CrossCutting.Dtos.Task;
using TaskManagement.CrossCutting.Notifications;
using TaskManagement.Domain.Aggregates.Task.Commands;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Domain.Aggregates.Task.Handlers
{
    public sealed class CreateTaskHandler(ITaskRepository taskRepository,
        IProjectRepository projectRepository,
        INotificationHandler notificationHandler) : CommandHandler(taskRepository.UnitOfWork), IRequestHandler<CreateTaskCommand, CreateTaskResponse>
    {
        public async Task<CreateTaskResponse> Handle(CreateTaskCommand request,
            CancellationToken cancellationToken)
        {
            var project = await projectRepository.GetByIdAsync(request.ProjectId, cancellationToken);
            if (project == null)
            {
                notificationHandler.AddNotification(Entities.Project.Errors.ProjectNotFound);
                return default!;
            }

            var task = new Entities.Task(request.ProjectId,
                request.Title,
                request.Description,
                request.DueDate,
                request.Priority);

            await taskRepository.AddAsync(task, cancellationToken);

            await CommitAsync(cancellationToken);

            return new(task.Id, 
                task.ProjectId, 
                task.Title, 
                task.DueDate, 
                task.Description, 
                task.Priority);
        }
    }
}
