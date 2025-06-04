using MediatR;
using Moq;
using TaskManagement.Application.Features.Tasks.Commands;
using TaskManagement.Application.Features.Tasks.Handlers;
using TaskManagement.CrossCutting.Notifications;
using TaskManagement.CrossCutting.Persistences;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.UnitTests.Domain.Aggregates.Task.Handlers;

public class DeleteTaskHandlerTests
{
    private readonly Mock<ITaskRepository> _taskRepositoryMock;
    private readonly Mock<INotificationHandler> _notificationHandlerMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly DeleteTaskHandler _handler;

    public DeleteTaskHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _taskRepositoryMock = new Mock<ITaskRepository>();
        _notificationHandlerMock = new Mock<INotificationHandler>();

        _taskRepositoryMock.Setup(repo => repo.UnitOfWork).Returns(_unitOfWorkMock.Object);

        _handler = new DeleteTaskHandler(
            _taskRepositoryMock.Object,
            _notificationHandlerMock.Object
        );
    }

    [Fact]
    public async System.Threading.Tasks.Task Handle_WhenTaskExists_ShouldDeleteTaskAndReturnUnitValue()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var command = new DeleteTaskCommand(taskId);

        var existingTask = new TaskManagement.Domain.Entities.Task(
            Guid.NewGuid(),
            "Task Title",
            "Task Description",
            DateTime.UtcNow.AddDays(1),
            TaskManagement.CrossCutting.Enums.TaskStatusEnum.Pending,
            TaskManagement.CrossCutting.Enums.TaskPriorityEnum.Medium
        );

        // Using reflection to set the task Id since it's a private setter
        typeof(TaskManagement.Domain.Entities.Task)
            .GetProperty("Id")
            ?.SetValue(existingTask, taskId);

        _taskRepositoryMock
            .Setup(repo => repo.FindAsync(taskId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingTask);

        _unitOfWorkMock
            .Setup(uow => uow.CommitAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(Unit.Value, result);
        _taskRepositoryMock.Verify(repo => repo.DeleteAsync(existingTask), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async System.Threading.Tasks.Task Handle_WhenTaskDoesNotExist_ShouldReturnDefaultAndAddNotification()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var command = new DeleteTaskCommand(taskId);

        _taskRepositoryMock
            .Setup(repo => repo.FindAsync(taskId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((TaskManagement.Domain.Entities.Task?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(default, result);
        _notificationHandlerMock.Verify(
            handler => handler.AddNotification(It.IsAny<Notification>()),
            Times.Once
        );
        _taskRepositoryMock.Verify(
            repo => repo.DeleteAsync(It.IsAny<TaskManagement.Domain.Entities.Task>()),
            Times.Never
        );
        _unitOfWorkMock.Verify(uow => uow.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
