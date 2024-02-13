using Core.Entity;

namespace Core.CacheRepository;

public interface ICacheRepository<TEntity> where TEntity : BaseEntity
{
    Task<TEntity> GetAsync(Guid id);
    Task SetAsync(Guid id, TEntity entity, TimeSpan? expiry = null);
    Task<bool> RemoveAsync(Guid id);
}