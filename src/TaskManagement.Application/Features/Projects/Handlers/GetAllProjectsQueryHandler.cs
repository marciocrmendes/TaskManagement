using MediatR;
using TaskManagement.Application.Common.Extensions;
using TaskManagement.Application.Features.Projects.Queries;
using TaskManagement.CrossCutting.Dtos;
using TaskManagement.CrossCutting.Dtos.Project;
using TaskManagement.Domain.Interfaces;
using DomainProject = TaskManagement.Domain.Entities.Project;

namespace TaskManagement.Application.Features.Projects.Handlers
{
    public class GetAllProjectsQueryHandler(IProjectRepository projectRepository)
        : IRequestHandler<GetAllProjectsQuery, GenericResponseList<ProjectResponse>>
    {
        public async Task<GenericResponseList<ProjectResponse>> Handle(
            GetAllProjectsQuery _,
            CancellationToken cancellationToken
        )
        {
            var projects = await projectRepository.GetAllAsync(cancellationToken);
            return new([.. projects.Select(project => project.ToResponse())]);
        }
    }
}
