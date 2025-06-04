using MediatR;
using TaskManagement.CrossCutting.Dtos.Project;

namespace TaskManagement.Application.Features.Projects.Commands
{
    public sealed record CreateProjectCommand(string Name, string? Description)
        : IRequest<CreateProjectResponse>;
}
