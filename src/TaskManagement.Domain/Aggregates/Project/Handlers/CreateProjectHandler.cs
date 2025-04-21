using MediatR;
using TaskManagement.CrossCutting.Dtos.Project;
using TaskManagement.Domain.Aggregates.Project.Commands;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Domain.Aggregates.Project.Handlers
{
    public sealed class CreateProjectHandler(IProjectRepository projectRepository) : 
        CommandHandler(projectRepository.UnitOfWork), 
        IRequestHandler<CreateProjectCommand, CreateProjectResponse>
    {
        public async Task<CreateProjectResponse> Handle(CreateProjectCommand request, 
            CancellationToken cancellationToken)
        {
            var existingProject = await projectRepository
                .ExistsAsync(x => x.Name.ToLower() ==  request.Name.ToLower(), cancellationToken);

            var project = new Entities.Project(request.Name, request.Description);

            await projectRepository.AddAsync(project, cancellationToken);

            await CommitAsync(cancellationToken);

            return new(project.Id, project.Name, project.Description);
        }
    }
}
