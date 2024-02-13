using Core.Entity;

namespace Core.DapperRepository;

public interface IDapperRepository<TEntity> where TEntity : BaseEntity
{
    Task<TEntity?> GetByIdAsync(Guid id);
    Task<IEnumerable<TEntity>> GetAllAsync();

    Task AddAsync(TEntity entity);

    Task Remove(Guid id);

    Task UpdateAsync(TEntity entity);
}