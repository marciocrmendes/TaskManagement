using Task = TaskManagement.Domain.Entities.Task;

namespace TaskManagement.Domain.Interfaces
{
    public interface ITaskRepository : IRepository<Task>
    {
    }
}