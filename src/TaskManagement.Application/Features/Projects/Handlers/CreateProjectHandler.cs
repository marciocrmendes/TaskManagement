using MediatR;
using TaskManagement.Application.Common;
using TaskManagement.Application.Features.Projects.Commands;
using TaskManagement.CrossCutting.Dtos.Project;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Application.Features.Projects.Handlers
{
    public sealed class CreateProjectHandler(IProjectRepository projectRepository)
        : CommandHandler(projectRepository.UnitOfWork),
            IRequestHandler<CreateProjectCommand, CreateProjectResponse>
    {
        public async Task<CreateProjectResponse> Handle(
            CreateProjectCommand request,
            CancellationToken cancellationToken
        )
        {
            var existingProject = await projectRepository.ExistsAsync(
                x => x.Name.ToLower() == request.Name.ToLower(),
                cancellationToken
            );

            var project = new Domain.Entities.Project(request.Name, request.Description);

            await projectRepository.AddAsync(project, cancellationToken);

            await CommitAsync(cancellationToken);

            return new(project.Id, project.Name, project.Description);
        }
    }
}
