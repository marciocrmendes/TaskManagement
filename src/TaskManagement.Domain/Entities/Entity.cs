using TaskManagement.CrossCutting.Events;

namespace TaskManagement.Domain.Entities
{
    public abstract class Entity
    {
        private readonly List<IDomainEvent> _domainEvents = [];

        public DateTime CreatedAt { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Guid? DeletedBy { get; set; }

        public bool IsDeleted => DeletedAt.HasValue;

        public IReadOnlyList<IDomainEvent> GetDomainEvents() => [.. _domainEvents];

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }

        protected void RaiseDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }
    }

    public abstract class Entity<TId> : Entity where TId : struct
    {
        public TId Id { get; set; }
    }
}
