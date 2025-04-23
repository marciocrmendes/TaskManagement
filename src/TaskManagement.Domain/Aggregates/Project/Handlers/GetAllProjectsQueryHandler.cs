using MediatR;
using TaskManagement.CrossCutting.Dtos;
using TaskManagement.CrossCutting.Dtos.Project;
using TaskManagement.Domain.Aggregates.Project.Queries;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Domain.Aggregates.Project.Handlers
{
    public class GetAllProjectsQueryHandler(IProjectRepository projectRepository) :
        IRequestHandler<GetAllProjectsQuery, GenericResponseList<ProjectResponse>>
    {
        public async Task<GenericResponseList<ProjectResponse>> Handle(GetAllProjectsQuery _, CancellationToken cancellationToken)
        {
            var projects = await projectRepository.GetAllAsync(cancellationToken);
            return new([.. projects.Select(project => project.ToResponse())]);
        }
    }
}
