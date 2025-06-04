namespace TaskManagement.Domain.Interfaces
{
    public interface ITaskRepository : IRepository<Entities.Task>
    {
        Task<Entities.Task?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
