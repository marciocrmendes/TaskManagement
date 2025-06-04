using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Infra.Context.Interceptors
{
    public sealed class PublishDomainEventsInterceptor(IPublisher publisher)
        : SaveChangesInterceptor
    {
        public override async ValueTask<int> SavedChangesAsync(
            SaveChangesCompletedEventData eventData,
            int result,
            CancellationToken cancellationToken = default
        )
        {
            if (eventData.Context is null)
                return result;

            await PublishDomainEventsAsync(eventData.Context, cancellationToken);
            return result;
        }

        private async System.Threading.Tasks.Task PublishDomainEventsAsync(
            DbContext context,
            CancellationToken cancellationToken
        )
        {
            var domainEvents = context
                .ChangeTracker.Entries<Entity>()
                .Select(entry => entry.Entity)
                .SelectMany(entity =>
                {
                    var domainEvents = entity.GetDomainEvents();
                    entity.ClearDomainEvents();
                    return domainEvents;
                })
                .ToArray();

            foreach (var domainEvent in domainEvents)
            {
                await publisher.Publish(domainEvent, cancellationToken);
            }
        }
    }
}
