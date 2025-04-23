using MediatR;
using TaskManagement.CrossCutting.Dtos;
using TaskManagement.CrossCutting.Dtos.Project;

namespace TaskManagement.Domain.Aggregates.Project.Queries
{
    public sealed record GetAllProjectsQuery() : IRequest<GenericResponseList<ProjectResponse>>
    {
    }
}
