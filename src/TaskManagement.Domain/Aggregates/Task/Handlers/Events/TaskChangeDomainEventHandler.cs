using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TaskManagement.Domain.Aggregates.Task.Events;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Domain.Aggregates.Task.Handlers.Events
{
    public sealed class TaskChangeDomainEventHandler : DomainEventHandler, INotificationHandler<AddTaskChangeDomainEvent>
    {
        public TaskChangeDomainEventHandler(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async System.Threading.Tasks.Task Handle(AddTaskChangeDomainEvent notification, CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider!.CreateScope();
            var taskHistoryRepository = scope.ServiceProvider.GetRequiredService<ITaskHistoryRepository>();

            var taskHistory = new TaskHistory(
                notification.TaskId,
                nameof(AddTaskChangeDomainEvent),
                notification.Data,
                Guid.NewGuid());

            await taskHistoryRepository.AddAsync(taskHistory, cancellationToken);
            await taskHistoryRepository.UnitOfWork.CommitAsync(cancellationToken);
        }
    }
}
