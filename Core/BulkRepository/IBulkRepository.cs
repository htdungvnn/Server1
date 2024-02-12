using Core.Entity;

namespace Core.BulkRepository;

public interface IBulkRepository<TEntity> where TEntity : BaseEntity
{
    Task BulkAddRangeAsync(IEnumerable<TEntity> entities);

    Task BulkRemoveRange(IEnumerable<Guid> ids);

    Task BulkUpdateAsync(IEnumerable<TEntity> entities);
    Task BulkSaveChange();
}