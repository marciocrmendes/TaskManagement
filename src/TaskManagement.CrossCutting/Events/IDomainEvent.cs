using MediatR;

namespace TaskManagement.CrossCutting.Events
{
    public interface IDomainEvent : INotification
    {
    }
}
