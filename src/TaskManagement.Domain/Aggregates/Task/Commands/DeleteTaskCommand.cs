using MediatR;

namespace TaskManagement.Domain.Aggregates.Task.Commands
{
    public record DeleteTaskCommand(Guid Id) : IRequest<Unit>;
}
