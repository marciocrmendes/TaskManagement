using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Text.Json;
using TaskManagement.CrossCutting.Persistences;
using TaskManagement.Domain.Aggregates.Task.Events;
using TaskManagement.Domain.Aggregates.Task.Handlers.Events;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.UnitTests.Domain.Aggregates.Task.Handlers.Events;

public class TaskChangeDomainEventHandlerTests
{
    private readonly Mock<IServiceProvider> _serviceProviderMock;
    private readonly Mock<IServiceScope> _serviceScopeMock;
    private readonly Mock<IServiceScopeFactory> _serviceScopeFactoryMock;
    private readonly Mock<ITaskHistoryRepository> _taskHistoryRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly TaskChangeDomainEventHandler _handler;

    public TaskChangeDomainEventHandlerTests()
    {
        _serviceProviderMock = new Mock<IServiceProvider>();
        _serviceScopeMock = new Mock<IServiceScope>();
        _serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();
        _taskHistoryRepositoryMock = new Mock<ITaskHistoryRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _taskHistoryRepositoryMock.Setup(repo => repo.UnitOfWork).Returns(_unitOfWorkMock.Object);

        // Configuração do Service Provider
        _serviceProviderMock
            .Setup(sp => sp.GetService(typeof(IServiceScopeFactory)))
            .Returns(_serviceScopeFactoryMock.Object);

        _serviceScopeFactoryMock
            .Setup(factory => factory.CreateScope())
            .Returns(_serviceScopeMock.Object);

        _serviceScopeMock
            .Setup(scope => scope.ServiceProvider)
            .Returns(_serviceProviderMock.Object);

        _serviceProviderMock
            .Setup(sp => sp.GetService(typeof(ITaskHistoryRepository)))
            .Returns(_taskHistoryRepositoryMock.Object);

        _handler = new TaskChangeDomainEventHandler(_serviceProviderMock.Object);
    }

    [Fact]
    public async System.Threading.Tasks.Task Handle_ShouldCreateTaskHistoryAndPersist()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var eventData = new { 
            OldValue = "Original Value", 
            NewValue = "Updated Value" 
        };
        var serializedData = JsonSerializer.Serialize(eventData);

        var notification = new AddTaskChangeDomainEvent(taskId, serializedData);

        var taskHistory = new TaskHistory(
            taskId,
            nameof(AddTaskChangeDomainEvent),
            JsonSerializer.Serialize(notification),
            userId);

        _taskHistoryRepositoryMock
            .Setup(repo => repo.AddAsync(It.IsAny<TaskHistory>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(taskHistory);

        _unitOfWorkMock
            .Setup(uow => uow.CommitAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        await _handler.Handle(notification, CancellationToken.None);

        // Assert
        _taskHistoryRepositoryMock.Verify(
            repo => repo.AddAsync(
                It.Is<TaskHistory>(h => 
                    h.TaskId == taskId && 
                    h.Event == nameof(AddTaskChangeDomainEvent) && 
                    h.Data == serializedData),
                It.IsAny<CancellationToken>()),
            Times.Once);
        
        _unitOfWorkMock.Verify(
            uow => uow.CommitAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }
}