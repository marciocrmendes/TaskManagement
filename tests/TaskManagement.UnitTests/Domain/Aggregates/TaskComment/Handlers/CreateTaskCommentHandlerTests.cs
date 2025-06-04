using System.Linq.Expressions;
using System.Text.Json;
using MediatR;
using Moq;
using TaskManagement.Application.Features.TaskComments.Commands;
using TaskManagement.Application.Features.TaskComments.Handlers;
using TaskManagement.CrossCutting.Notifications;
using TaskManagement.CrossCutting.Persistences;
using TaskManagement.Domain.Aggregates.TaskComment.Events;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.UnitTests.Domain.Aggregates.TaskComment.Handlers;

public class CreateTaskCommentHandlerTests
{
    private readonly Mock<ITaskHistoryRepository> _taskHistoryRepositoryMock;
    private readonly Mock<ITaskRepository> _taskRepositoryMock;
    private readonly Mock<INotificationHandler> _notificationHandlerMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly CreateTaskCommentHandler _handler;

    public CreateTaskCommentHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _taskHistoryRepositoryMock = new Mock<ITaskHistoryRepository>();
        _taskRepositoryMock = new Mock<ITaskRepository>();
        _notificationHandlerMock = new Mock<INotificationHandler>();

        _taskHistoryRepositoryMock.Setup(repo => repo.UnitOfWork).Returns(_unitOfWorkMock.Object);

        _handler = new CreateTaskCommentHandler(
            _taskHistoryRepositoryMock.Object,
            _taskRepositoryMock.Object,
            _notificationHandlerMock.Object
        );
    }

    [Fact]
    public async System.Threading.Tasks.Task Handle_WhenTaskExists_ShouldCreateCommentAndReturnUnitValue()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var comment = "This is a test comment";
        var command = new CreateTaskCommentCommand { TaskId = taskId, Comment = comment };

        var taskHistory = new TaskHistory(
            taskId,
            nameof(AddTaskCommentDomainEvent),
            JsonSerializer.Serialize(new AddTaskCommentDomainEvent(comment)),
            userId
        );

        _taskRepositoryMock
            .Setup(repo =>
                repo.ExistsAsync(
                    It.IsAny<Expression<Func<TaskManagement.Domain.Entities.Task, bool>>>(),
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync(true);

        _taskHistoryRepositoryMock
            .Setup(repo => repo.AddAsync(It.IsAny<TaskHistory>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(taskHistory);

        _unitOfWorkMock
            .Setup(uow => uow.CommitAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(Unit.Value, result);

        _taskHistoryRepositoryMock.Verify(
            repo =>
                repo.AddAsync(
                    It.Is<TaskHistory>(h =>
                        h.TaskId == taskId && h.Event == nameof(AddTaskCommentDomainEvent)
                    ),
                    It.IsAny<CancellationToken>()
                ),
            Times.Once
        );

        _unitOfWorkMock.Verify(uow => uow.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async System.Threading.Tasks.Task Handle_WhenTaskDoesNotExist_ShouldReturnDefaultAndAddNotification()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var command = new CreateTaskCommentCommand
        {
            TaskId = taskId,
            Comment = "This is a test comment",
        };

        _taskRepositoryMock
            .Setup(repo =>
                repo.ExistsAsync(
                    It.IsAny<Expression<Func<TaskManagement.Domain.Entities.Task, bool>>>(),
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(default, result);

        _notificationHandlerMock.Verify(
            handler => handler.AddNotification(It.IsAny<Notification>()),
            Times.Once
        );

        _taskHistoryRepositoryMock.Verify(
            repo => repo.AddAsync(It.IsAny<TaskHistory>(), It.IsAny<CancellationToken>()),
            Times.Never
        );

        _unitOfWorkMock.Verify(uow => uow.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
