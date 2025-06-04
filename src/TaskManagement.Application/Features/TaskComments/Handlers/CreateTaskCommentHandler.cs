using System.Text.Json;
using MediatR;
using TaskManagement.Application.Common;
using TaskManagement.Application.Features.TaskComments.Commands;
using TaskManagement.CrossCutting.Notifications;
using TaskManagement.Domain.Aggregates.TaskComment.Events;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Application.Features.TaskComments.Handlers
{
    public sealed class CreateTaskCommentHandler(
        ITaskHistoryRepository taskHistoryRepository,
        ITaskRepository taskRepository,
        INotificationHandler notificationHandler
    )
        : CommandHandler(taskHistoryRepository.UnitOfWork),
            IRequestHandler<CreateTaskCommentCommand, Unit>
    {
        public async Task<Unit> Handle(
            CreateTaskCommentCommand request,
            CancellationToken cancellationToken
        )
        {
            var exists = await taskRepository.ExistsAsync(
                x => x.Id == request.TaskId,
                cancellationToken
            );
            if (!exists)
            {
                notificationHandler.AddNotification(Domain.Entities.Task.Errors.NotFound);
                return default!;
            }

            var data = new AddTaskCommentDomainEvent(request.Comment);

            var taskHistory = new TaskHistory(
                request.TaskId,
                nameof(AddTaskCommentDomainEvent),
                JsonSerializer.Serialize(data),
                Guid.NewGuid()
            );

            await taskHistoryRepository.AddAsync(taskHistory, cancellationToken);

            await CommitAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
