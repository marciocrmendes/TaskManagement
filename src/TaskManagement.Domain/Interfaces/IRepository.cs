using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TaskManagement.CrossCutting.Persistences;
using TaskManagement.Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace TaskManagement.Domain.Interfaces
{
    public interface IRepository<TEntity> where TEntity : Entity
    {
        DbSet<TEntity> Table { get; }
        IUnitOfWork UnitOfWork { get; }
        Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task AddRangeAsync(ICollection<TEntity> entities, CancellationToken cancellationToken = default);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task<TEntity?> FindAsync<TId>(TId id, CancellationToken cancellationToken);
        Task<IReadOnlyCollection<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate,
             CancellationToken cancellationToken = default);        
        Task<IReadOnlyCollection<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> predicate,
             CancellationToken cancellationToken = default);
    }
}