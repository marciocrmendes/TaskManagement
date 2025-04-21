using MediatR;
using TaskManagement.CrossCutting.Dtos.Project;

namespace TaskManagement.Domain.Aggregates.Project.Commands
{
    public sealed record CreateProjectCommand(string Name, string? Description) : IRequest<CreateProjectResponse>;
}
