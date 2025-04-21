using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TaskManagement.CrossCutting.Persistences;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Infra.Context;
using Task = System.Threading.Tasks.Task;

namespace TaskManagement.Infra.Repositories
{
    public abstract class Repository<TEntity>(TaskManagementDbContext dbContext) : 
        IRepository<TEntity> where TEntity : Entity
    {
        public IUnitOfWork UnitOfWork => dbContext;

        public virtual async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await dbContext.Set<TEntity>().AddAsync(entity, cancellationToken);
            return entity;
        }

        public virtual async Task AddRangeAsync(ICollection<TEntity> entities, CancellationToken cancellationToken = default)
        {
            await dbContext.Set<TEntity>().AddRangeAsync(entities, cancellationToken);
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            dbContext.Set<TEntity>().Update(entity);
            await Task.CompletedTask;
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            dbContext.Set<TEntity>().Remove(entity);
            await Task.CompletedTask;
        }

        public virtual async Task<TEntity?> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken = default)
        {
            return await dbContext.Set<TEntity>().FindAsync([id], cancellationToken);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await dbContext.Set<TEntity>()
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            return await dbContext.Set<TEntity>()
                .AsNoTracking()
                .AnyAsync(predicate, cancellationToken);
        }

        public virtual async Task<IEnumerable<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            return await dbContext.Set<TEntity>()
                .AsNoTracking()
                .Where(predicate)
                .ToListAsync(cancellationToken);
        }
    }
}
