using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TaskManagement.CrossCutting.Persistences;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Infra.Context;
using Task = System.Threading.Tasks.Task;

namespace TaskManagement.Infra.Repositories
{
    public abstract class Repository<TEntity>(TaskManagementDbContext dbContext)
        : IRepository<TEntity>
        where TEntity : Entity
    {
        public DbSet<TEntity> Table => dbContext.Set<TEntity>();
        public IUnitOfWork UnitOfWork => dbContext;

        public virtual async Task<TEntity> AddAsync(
            TEntity entity,
            CancellationToken cancellationToken = default
        )
        {
            await Table.AddAsync(entity, cancellationToken);
            return entity;
        }

        public virtual async Task AddRangeAsync(
            ICollection<TEntity> entities,
            CancellationToken cancellationToken = default
        )
        {
            await Table.AddRangeAsync(entities, cancellationToken);
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            Table.Update(entity);
            await Task.CompletedTask;
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            Table.Remove(entity);
            await Task.CompletedTask;
        }

        public virtual async Task<TEntity?> FindAsync<TId>(
            TId id,
            CancellationToken cancellationToken = default
        )
        {
            return await Table.FindAsync([id], cancellationToken);
        }

        public virtual async Task<IReadOnlyCollection<TEntity>> GetAllAsync(
            CancellationToken cancellationToken = default
        )
        {
            return await Table.AsNoTracking().ToListAsync(cancellationToken);
        }

        public virtual async Task<bool> ExistsAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default
        )
        {
            return await Table.AsNoTracking().AnyAsync(predicate, cancellationToken);
        }

        public virtual async Task<IReadOnlyCollection<TEntity>> WhereAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default
        )
        {
            return await Table.AsNoTracking().Where(predicate).ToListAsync(cancellationToken);
        }
    }
}
