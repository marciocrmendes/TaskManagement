using System.Linq.Expressions;
using Moq;
using TaskManagement.Application.Features.Projects.Handlers;
using TaskManagement.Application.Features.Projects.Queries;
using TaskManagement.CrossCutting.Enums;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.UnitTests.Domain.Aggregates.Project.Handlers;

public class GetTasksByIdQueryHandlerTests
{
    private readonly Mock<ITaskRepository> _taskRepositoryMock;
    private readonly GetTasksByIdQueryHandler _handler;

    public GetTasksByIdQueryHandlerTests()
    {
        _taskRepositoryMock = new Mock<ITaskRepository>();
        _handler = new GetTasksByIdQueryHandler(_taskRepositoryMock.Object);
    }

    [Fact]
    public async System.Threading.Tasks.Task Handle_ShouldReturnTasksForSpecificProject()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var tasks = new List<TaskManagement.Domain.Entities.Task>
        {
            CreateTask(projectId, "Task 1", "Description 1", TaskStatusEnum.Pending),
            CreateTask(projectId, "Task 2", "Description 2", TaskStatusEnum.InProgress),
            CreateTask(projectId, "Task 3", "Description 3", TaskStatusEnum.Completed),
        };

        _taskRepositoryMock
            .Setup(repo =>
                repo.WhereAsync(
                    It.IsAny<Expression<Func<TaskManagement.Domain.Entities.Task, bool>>>(),
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync(tasks);

        var query = new GetTasksByIdQuery(projectId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(tasks.Count, result.Items.Count);
    }

    [Fact]
    public async System.Threading.Tasks.Task Handle_WhenNoTasksForProject_ShouldReturnEmptyList()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var tasks = new List<TaskManagement.Domain.Entities.Task>();

        _taskRepositoryMock
            .Setup(repo =>
                repo.WhereAsync(
                    It.IsAny<Expression<Func<TaskManagement.Domain.Entities.Task, bool>>>(),
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync(tasks);

        var query = new GetTasksByIdQuery(projectId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result.Items);
    }

    private TaskManagement.Domain.Entities.Task CreateTask(
        Guid projectId,
        string title,
        string description,
        TaskStatusEnum status
    )
    {
        var task = new TaskManagement.Domain.Entities.Task(
            projectId,
            title,
            description,
            DateTime.UtcNow.AddDays(7),
            status,
            TaskPriorityEnum.Medium
        );

        // Definir um ID para a tarefa usando reflection (já que o setter é privado)
        typeof(TaskManagement.Domain.Entities.Task)
            .GetProperty("Id")
            ?.SetValue(task, Guid.NewGuid());

        return task;
    }
}
