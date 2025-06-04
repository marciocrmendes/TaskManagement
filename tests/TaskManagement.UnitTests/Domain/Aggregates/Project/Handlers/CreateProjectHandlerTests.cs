using System.Linq.Expressions;
using Moq;
using TaskManagement.Application.Features.Projects.Commands;
using TaskManagement.Application.Features.Projects.Handlers;
using TaskManagement.CrossCutting.Dtos.Project;
using TaskManagement.CrossCutting.Persistences;
using TaskManagement.Domain.Interfaces;
using Xunit;

namespace TaskManagement.UnitTests.Domain.Aggregates.Project.Handlers;

public class CreateProjectHandlerTests
{
    private readonly Mock<IProjectRepository> _projectRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly CreateProjectHandler _handler;

    public CreateProjectHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _projectRepositoryMock = new Mock<IProjectRepository>();

        _projectRepositoryMock.Setup(repo => repo.UnitOfWork).Returns(_unitOfWorkMock.Object);

        _handler = new CreateProjectHandler(_projectRepositoryMock.Object);
    }

    [Fact]
    public async System.Threading.Tasks.Task Handle_ShouldCreateProjectAndReturnResponse()
    {
        // Arrange
        var projectName = "Test Project";
        var projectDescription = "Test Description";

        var command = new CreateProjectCommand(projectName, projectDescription);

        _projectRepositoryMock
            .Setup(repo =>
                repo.ExistsAsync(
                    It.IsAny<Expression<Func<TaskManagement.Domain.Entities.Project, bool>>>(),
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync(false);

        _projectRepositoryMock
            .Setup(repo =>
                repo.AddAsync(
                    It.IsAny<TaskManagement.Domain.Entities.Project>(),
                    It.IsAny<CancellationToken>()
                )
            )
            .Callback<TaskManagement.Domain.Entities.Project, CancellationToken>(
                (project, _) =>
                {
                    // Simular a geração de ID como faria o repositório real
                    typeof(TaskManagement.Domain.Entities.Project)
                        .GetProperty("Id")
                        ?.SetValue(project, Guid.NewGuid());
                }
            );

        _unitOfWorkMock
            .Setup(uow => uow.CommitAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(projectName, result.Name);
        Assert.Equal(projectDescription, result.Description);

        _projectRepositoryMock.Verify(
            repo =>
                repo.AddAsync(
                    It.Is<TaskManagement.Domain.Entities.Project>(p =>
                        p.Name == projectName && p.Description == projectDescription
                    ),
                    It.IsAny<CancellationToken>()
                ),
            Times.Once
        );

        _unitOfWorkMock.Verify(uow => uow.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async System.Threading.Tasks.Task Handle_WhenProjectNameExists_ShouldStillCreateProject()
    {
        // Arrange
        var projectName = "Existing Project";
        var projectDescription = "Test Description";

        var command = new CreateProjectCommand(projectName, projectDescription);

        _projectRepositoryMock
            .Setup(repo =>
                repo.ExistsAsync(
                    It.IsAny<Expression<Func<TaskManagement.Domain.Entities.Project, bool>>>(),
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync(true); // Projeto com mesmo nome existe

        _projectRepositoryMock
            .Setup(repo =>
                repo.AddAsync(
                    It.IsAny<TaskManagement.Domain.Entities.Project>(),
                    It.IsAny<CancellationToken>()
                )
            )
            .Callback<TaskManagement.Domain.Entities.Project, CancellationToken>(
                (project, _) =>
                {
                    // Simular a geração de ID como faria o repositório real
                    typeof(TaskManagement.Domain.Entities.Project)
                        .GetProperty("Id")
                        ?.SetValue(project, Guid.NewGuid());
                }
            );

        _unitOfWorkMock
            .Setup(uow => uow.CommitAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(projectName, result.Name);
        Assert.Equal(projectDescription, result.Description);

        _projectRepositoryMock.Verify(
            repo =>
                repo.AddAsync(
                    It.Is<TaskManagement.Domain.Entities.Project>(p =>
                        p.Name == projectName && p.Description == projectDescription
                    ),
                    It.IsAny<CancellationToken>()
                ),
            Times.Once
        );

        _unitOfWorkMock.Verify(uow => uow.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
