using Moq;
using System.Linq.Expressions;
using TaskManagement.CrossCutting.Enums;
using TaskManagement.CrossCutting.Notifications;
using TaskManagement.CrossCutting.Persistences;
using TaskManagement.Domain.Aggregates.Task.Commands;
using TaskManagement.Domain.Aggregates.Task.Handlers;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.UnitTests.Domain.Aggregates.Task.Handlers;

public class UpdateTaskHandlerTests
{
    private readonly Mock<ITaskRepository> _taskRepositoryMock;
    private readonly Mock<IProjectRepository> _projectRepositoryMock;
    private readonly Mock<INotificationHandler> _notificationHandlerMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly UpdateTaskHandler _handler;

    public UpdateTaskHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _taskRepositoryMock = new Mock<ITaskRepository>();
        _projectRepositoryMock = new Mock<IProjectRepository>();
        _notificationHandlerMock = new Mock<INotificationHandler>();

        _taskRepositoryMock.Setup(repo => repo.UnitOfWork).Returns(_unitOfWorkMock.Object);
        
        _handler = new UpdateTaskHandler(
            _taskRepositoryMock.Object,
            _projectRepositoryMock.Object,
            _notificationHandlerMock.Object);
    }

    [Fact]
    public async System.Threading.Tasks.Task Handle_WhenTaskAndProjectExist_ShouldUpdateTask()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var projectId = Guid.NewGuid();
        var command = new UpdateTaskCommand(
            taskId,
            projectId,
            "Updated Task Title",
            "Updated Description",
            DateTime.UtcNow.AddDays(2),
            TaskStatusEnum.InProgress
        );

        var existingTask = new TaskManagement.Domain.Entities.Task(
            projectId,
            "Original Title",
            "Original Description",
            DateTime.UtcNow.AddDays(1),
            TaskStatusEnum.Pending,
            TaskPriorityEnum.Medium);

        // Using reflection to set the task Id since it's a private setter
        typeof(TaskManagement.Domain.Entities.Task)
            .GetProperty("Id")
            ?.SetValue(existingTask, taskId);

        _taskRepositoryMock
            .Setup(repo => repo.GetByIdAsync(taskId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingTask);

        _projectRepositoryMock
            .Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<TaskManagement.Domain.Entities.Project, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _unitOfWorkMock
            .Setup(uow => uow.CommitAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        _taskRepositoryMock.Verify(repo => repo.UpdateAsync(existingTask), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async System.Threading.Tasks.Task Handle_WhenTaskDoesNotExist_ShouldReturnDefaultAndAddNotification()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var command = new UpdateTaskCommand(
            taskId,
            Guid.NewGuid(),
            "Updated Task Title",
            "Updated Description",
            DateTime.UtcNow.AddDays(2),
            TaskStatusEnum.InProgress
        );

        _taskRepositoryMock
            .Setup(repo => repo.GetByIdAsync(taskId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((TaskManagement.Domain.Entities.Task)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Null(result);
        _notificationHandlerMock.Verify(handler => 
            handler.AddNotification(It.IsAny<Notification>()), Times.Once);
        _taskRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<TaskManagement.Domain.Entities.Task>()), Times.Never);
        _unitOfWorkMock.Verify(uow => uow.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async System.Threading.Tasks.Task Handle_WhenProjectDoesNotExist_ShouldReturnDefaultAndAddNotification()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var projectId = Guid.NewGuid();
        var command = new UpdateTaskCommand(
            taskId,
            Guid.NewGuid(),
            "Updated Task Title",
            "Updated Description",
            DateTime.UtcNow.AddDays(2),
            TaskStatusEnum.InProgress
        );

        var existingTask = new TaskManagement.Domain.Entities.Task(
            projectId,
            "Original Title",
            "Original Description",
            DateTime.UtcNow.AddDays(1),
            TaskStatusEnum.Pending,
            TaskPriorityEnum.Medium);

        // Using reflection to set the task Id since it's a private setter
        typeof(TaskManagement.Domain.Entities.Task)
            .GetProperty("Id")
            ?.SetValue(existingTask, taskId);

        _taskRepositoryMock
            .Setup(repo => repo.GetByIdAsync(taskId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingTask);

        _projectRepositoryMock
            .Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<TaskManagement.Domain.Entities.Project, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Null(result);
        _notificationHandlerMock.Verify(handler => 
            handler.AddNotification(It.IsAny<Notification>()), Times.Once);
        _taskRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<TaskManagement.Domain.Entities.Task>()), Times.Never);
        _unitOfWorkMock.Verify(uow => uow.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}