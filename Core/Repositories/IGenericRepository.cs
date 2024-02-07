using System.Linq.Expressions;
using Core.Entity;

namespace Core.Repositories;

public interface IGenericRepository<TEntity> where TEntity : BaseEntity
{
    Task<TEntity?> GetByIdAsync(Guid id);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);

    Task AddAsync(TEntity entity);
    Task AddRangeAsync(IEnumerable<TEntity> entities);

    Task Remove(Guid id);
    Task RemoveRange(IEnumerable<Guid> ids);

    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);
    Task UpdateAsync(TEntity entity);
}