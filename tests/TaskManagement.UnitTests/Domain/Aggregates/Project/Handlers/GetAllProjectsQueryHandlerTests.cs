using Moq;
using TaskManagement.Application.Features.Projects.Handlers;
using TaskManagement.Application.Features.Projects.Queries;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.UnitTests.Domain.Aggregates.Project.Handlers;

public class GetAllProjectsQueryHandlerTests
{
    private readonly Mock<IProjectRepository> _projectRepositoryMock;
    private readonly GetAllProjectsQueryHandler _handler;

    public GetAllProjectsQueryHandlerTests()
    {
        _projectRepositoryMock = new Mock<IProjectRepository>();
        _handler = new GetAllProjectsQueryHandler(_projectRepositoryMock.Object);
    }

    [Fact]
    public async System.Threading.Tasks.Task Handle_ShouldReturnAllProjects()
    {
        // Arrange
        var projects = new List<TaskManagement.Domain.Entities.Project>
        {
            CreateProject("Project 1", "Description 1"),
            CreateProject("Project 2", "Description 2"),
            CreateProject("Project 3", "Description 3"),
        };

        _projectRepositoryMock
            .Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(projects);

        var query = new GetAllProjectsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(projects.Count, result.Items.Count);
    }

    [Fact]
    public async System.Threading.Tasks.Task Handle_WhenNoProjects_ShouldReturnEmptyList()
    {
        // Arrange
        var projects = new List<TaskManagement.Domain.Entities.Project>();

        _projectRepositoryMock
            .Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(projects);

        var query = new GetAllProjectsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result.Items);
    }

    private TaskManagement.Domain.Entities.Project CreateProject(string name, string description)
    {
        var project = new TaskManagement.Domain.Entities.Project(name, description);

        // Definir um ID para o projeto usando reflection (já que o setter é privado)
        typeof(TaskManagement.Domain.Entities.Project)
            .GetProperty("Id")
            ?.SetValue(project, Guid.NewGuid());

        return project;
    }
}
