using System.Linq.Expressions;
using MediatR;
using Moq;
using TaskManagement.Application.Features.Projects.Commands;
using TaskManagement.Application.Features.Projects.Handlers;
using TaskManagement.CrossCutting.Notifications;
using TaskManagement.CrossCutting.Persistences;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.UnitTests.Domain.Aggregates.Project.Handlers;

public class DeleteProjectHandlerTests
{
    private readonly Mock<IProjectRepository> _projectRepositoryMock;
    private readonly Mock<ITaskRepository> _taskRepositoryMock;
    private readonly Mock<INotificationHandler> _notificationHandlerMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly DeleteProjectHandler _handler;

    public DeleteProjectHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _projectRepositoryMock = new Mock<IProjectRepository>();
        _taskRepositoryMock = new Mock<ITaskRepository>();
        _notificationHandlerMock = new Mock<INotificationHandler>();

        _projectRepositoryMock.Setup(repo => repo.UnitOfWork).Returns(_unitOfWorkMock.Object);

        _handler = new DeleteProjectHandler(
            _projectRepositoryMock.Object,
            _taskRepositoryMock.Object,
            _notificationHandlerMock.Object
        );
    }

    [Fact]
    public async System.Threading.Tasks.Task Handle_WhenProjectExistsAndHasNoPendingTasks_ShouldDeleteProjectAndReturnUnitValue()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var command = new DeleteProjectCommand(projectId);

        var existingProject = new TaskManagement.Domain.Entities.Project(
            "Test Project",
            "Test Description"
        );
        typeof(TaskManagement.Domain.Entities.Project)
            .GetProperty("Id")
            ?.SetValue(existingProject, projectId);

        _projectRepositoryMock
            .Setup(repo => repo.FindAsync(projectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProject);

        _taskRepositoryMock
            .Setup(repo =>
                repo.ExistsAsync(
                    It.IsAny<Expression<Func<TaskManagement.Domain.Entities.Task, bool>>>(),
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync(false); // Não tem tarefas pendentes

        _unitOfWorkMock
            .Setup(uow => uow.CommitAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(Unit.Value, result);
        _projectRepositoryMock.Verify(repo => repo.DeleteAsync(existingProject), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async System.Threading.Tasks.Task Handle_WhenProjectDoesNotExist_ShouldReturnDefaultAndAddNotification()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var command = new DeleteProjectCommand(projectId);

        _projectRepositoryMock
            .Setup(repo => repo.FindAsync(projectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((TaskManagement.Domain.Entities.Project)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(default, result);
        _notificationHandlerMock.Verify(
            handler => handler.AddNotification(It.IsAny<Notification>()),
            Times.Once
        );
        _projectRepositoryMock.Verify(
            repo => repo.DeleteAsync(It.IsAny<TaskManagement.Domain.Entities.Project>()),
            Times.Never
        );
        _unitOfWorkMock.Verify(uow => uow.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async System.Threading.Tasks.Task Handle_WhenProjectHasPendingTasks_ShouldReturnDefaultAndAddNotification()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var command = new DeleteProjectCommand(projectId);

        var existingProject = new TaskManagement.Domain.Entities.Project(
            "Test Project",
            "Test Description"
        );
        typeof(TaskManagement.Domain.Entities.Project)
            .GetProperty("Id")
            ?.SetValue(existingProject, projectId);

        _projectRepositoryMock
            .Setup(repo => repo.FindAsync(projectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProject);

        _taskRepositoryMock
            .Setup(repo =>
                repo.ExistsAsync(
                    It.IsAny<Expression<Func<TaskManagement.Domain.Entities.Task, bool>>>(),
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync(true); // Tem tarefas pendentes

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(default, result);
        _notificationHandlerMock.Verify(
            handler => handler.AddNotification(It.IsAny<Notification>()),
            Times.Once
        );
        _projectRepositoryMock.Verify(
            repo => repo.DeleteAsync(It.IsAny<TaskManagement.Domain.Entities.Project>()),
            Times.Never
        );
        _unitOfWorkMock.Verify(uow => uow.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
