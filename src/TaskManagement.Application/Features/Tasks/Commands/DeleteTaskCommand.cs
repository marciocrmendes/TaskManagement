using MediatR;

namespace TaskManagement.Application.Features.Tasks.Commands
{
    public record DeleteTaskCommand(Guid Id) : IRequest<Unit>;
}
