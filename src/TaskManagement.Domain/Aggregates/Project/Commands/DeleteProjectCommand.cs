using MediatR;

namespace TaskManagement.Domain.Aggregates.Project.Commands
{
    public sealed record DeleteProjectCommand(Guid Id) : IRequest<Unit>;
}
