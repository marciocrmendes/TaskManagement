using System.Linq.Expressions;
using Moq;
using TaskManagement.Application.Features.Tasks.Commands;
using TaskManagement.Application.Features.Tasks.Handlers;
using TaskManagement.CrossCutting.Enums;
using TaskManagement.CrossCutting.Notifications;
using TaskManagement.CrossCutting.Persistences;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.UnitTests.Domain.Aggregates.Task.Handlers;

public class CreateTaskHandlerTests
{
    private readonly Mock<ITaskRepository> _taskRepositoryMock;
    private readonly Mock<IProjectRepository> _projectRepositoryMock;
    private readonly Mock<INotificationHandler> _notificationHandlerMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly CreateTaskHandler _handler;

    public CreateTaskHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _taskRepositoryMock = new Mock<ITaskRepository>();
        _projectRepositoryMock = new Mock<IProjectRepository>();
        _notificationHandlerMock = new Mock<INotificationHandler>();

        _taskRepositoryMock.Setup(repo => repo.UnitOfWork).Returns(_unitOfWorkMock.Object);

        _handler = new CreateTaskHandler(
            _taskRepositoryMock.Object,
            _projectRepositoryMock.Object,
            _notificationHandlerMock.Object
        );
    }

    [Fact]
    public async System.Threading.Tasks.Task Handle_WhenProjectExists_AndNotAtTaskLimit_ShouldCreateTask()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var command = new CreateTaskCommand(
            projectId,
            "Test Task",
            "Test Description",
            DateTime.UtcNow.AddDays(1),
            TaskStatusEnum.Pending,
            TaskPriorityEnum.Medium
        );

        _projectRepositoryMock
            .Setup(repo =>
                repo.ExistsAsync(
                    It.IsAny<Expression<Func<TaskManagement.Domain.Entities.Project, bool>>>(),
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync(true);

        _projectRepositoryMock
            .Setup(repo => repo.GetTasksCountByIdAsync(projectId))
            .ReturnsAsync(5); // Menos que o limite de 20

        _taskRepositoryMock
            .Setup(repo =>
                repo.AddAsync(
                    It.IsAny<TaskManagement.Domain.Entities.Task>(),
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync(
                (TaskManagement.Domain.Entities.Task task, CancellationToken _) =>
                {
                    task.GetType().GetProperty("Id")?.SetValue(task, Guid.NewGuid());
                    return task;
                }
            );

        _unitOfWorkMock
            .Setup(uow => uow.CommitAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(command.Title, result.Title);
        Assert.Equal(command.ProjectId, result.ProjectId);
        Assert.Equal(command.Description, result.Description);
        Assert.Equal(command.DueDate, result.DueDate);
        Assert.Equal(command.Priority, result.Priority);
    }

    [Fact]
    public async System.Threading.Tasks.Task Handle_WhenProjectDoesNotExist_ShouldReturnDefaultAndAddNotification()
    {
        // Arrange
        var command = new CreateTaskCommand(
            Guid.NewGuid(),
            "Test Task",
            "Test Description",
            DateTime.UtcNow.AddDays(1),
            TaskStatusEnum.Pending,
            TaskPriorityEnum.Medium
        );

        _projectRepositoryMock
            .Setup(repo =>
                repo.ExistsAsync(
                    It.IsAny<Expression<Func<TaskManagement.Domain.Entities.Project, bool>>>(),
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Null(result);
        _notificationHandlerMock.Verify(
            handler => handler.AddNotification(It.IsAny<Notification>()),
            Times.Once
        );
        _taskRepositoryMock.Verify(
            repo =>
                repo.AddAsync(
                    It.IsAny<TaskManagement.Domain.Entities.Task>(),
                    It.IsAny<CancellationToken>()
                ),
            Times.Never
        );
    }

    [Fact]
    public async System.Threading.Tasks.Task Handle_WhenProjectAtTaskLimit_ShouldReturnDefaultAndAddNotification()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var command = new CreateTaskCommand(
            projectId,
            "Test Task",
            "Test Description",
            DateTime.UtcNow.AddDays(1),
            TaskStatusEnum.Pending,
            TaskPriorityEnum.Medium
        );

        _projectRepositoryMock
            .Setup(repo =>
                repo.ExistsAsync(
                    It.IsAny<Expression<Func<TaskManagement.Domain.Entities.Project, bool>>>(),
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync(true);

        _projectRepositoryMock
            .Setup(repo => repo.GetTasksCountByIdAsync(projectId))
            .ReturnsAsync(20); // No limite máximo

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Null(result);
        _notificationHandlerMock.Verify(
            handler => handler.AddNotification(It.IsAny<Notification>()),
            Times.Once
        );
        _taskRepositoryMock.Verify(
            repo =>
                repo.AddAsync(
                    It.IsAny<TaskManagement.Domain.Entities.Task>(),
                    It.IsAny<CancellationToken>()
                ),
            Times.Never
        );
    }
}
