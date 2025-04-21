using System.Linq.Expressions;
using TaskManagement.CrossCutting.Persistences;
using TaskManagement.Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace TaskManagement.Domain.Interfaces
{
    public interface IRepository<TEntity> where TEntity : Entity
    {
        IUnitOfWork UnitOfWork { get; }
        Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task AddRangeAsync(ICollection<TEntity> entities, CancellationToken cancellationToken = default);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task<TEntity?> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken);
        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate,
             CancellationToken cancellationToken = default);        
        Task<IEnumerable<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> predicate,
             CancellationToken cancellationToken = default);
    }
}