using MediatR;
using TaskManagement.CrossCutting.Dtos;
using TaskManagement.CrossCutting.Dtos.Project;

namespace TaskManagement.Application.Features.Projects.Queries
{
    public sealed record GetAllProjectsQuery() : IRequest<GenericResponseList<ProjectResponse>> { }
}
