using MediatR;

namespace TaskManagement.Application.Features.Projects.Commands
{
    public sealed record DeleteProjectCommand(Guid Id) : IRequest<Unit>;
}
