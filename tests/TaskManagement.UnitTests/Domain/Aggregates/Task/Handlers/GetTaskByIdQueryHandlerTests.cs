using Moq;
using TaskManagement.CrossCutting.Enums;
using TaskManagement.Domain.Aggregates.Task.Handlers;
using TaskManagement.Domain.Aggregates.Task.Queries;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.UnitTests.Domain.Aggregates.Task.Handlers;

public class GetTaskByIdQueryHandlerTests
{
    private readonly Mock<ITaskRepository> _taskRepositoryMock;
    private readonly GetTaskByIdQueryHandler _handler;

    public GetTaskByIdQueryHandlerTests()
    {
        _taskRepositoryMock = new Mock<ITaskRepository>();
        _handler = new GetTaskByIdQueryHandler(_taskRepositoryMock.Object);
    }

    [Fact]
    public async System.Threading.Tasks.Task Handle_WhenTaskExists_ShouldReturnTaskResponse()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var projectId = Guid.NewGuid();
        var query = new GetTaskByIdQuery(taskId);

        var existingTask = new TaskManagement.Domain.Entities.Task(
            projectId,
            "Task Title",
            "Task Description",
            DateTime.UtcNow.AddDays(1),
            TaskStatusEnum.Pending,
            TaskPriorityEnum.Medium);

        // Using reflection to set the task Id since it's a private setter
        typeof(TaskManagement.Domain.Entities.Task)
            .GetProperty("Id")
            ?.SetValue(existingTask, taskId);

        _taskRepositoryMock
            .Setup(repo => repo.FindAsync(taskId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingTask);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(taskId, result.Id);
        Assert.Equal(projectId, result.ProjectId);
        Assert.Equal("Task Title", result.Title);
        Assert.Equal("Task Description", result.Description);
    }

    [Fact]
    public async System.Threading.Tasks.Task Handle_WhenTaskDoesNotExist_ShouldReturnDefault()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var query = new GetTaskByIdQuery(taskId);

        _taskRepositoryMock
            .Setup(repo => repo.FindAsync(taskId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((TaskManagement.Domain.Entities.Task?)null);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Null(result);
    }
}