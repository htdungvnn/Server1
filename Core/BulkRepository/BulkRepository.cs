using Core.Entity;
using Microsoft.EntityFrameworkCore;

namespace Core.BulkRepository;

public class BulkRepository<TEntity> : IBulkRepository<TEntity> where TEntity : BaseEntity
{
    private readonly DbContext _context;
    private readonly DbSet<TEntity> _dbSet;

    public BulkRepository(DbContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    public async Task BulkAddRangeAsync(IEnumerable<TEntity> entities)
    {
        await _dbSet.BulkInsertAsync(entities);
        await _context.BulkSaveChangesAsync();
    }

    public async Task BulkRemoveRange(IEnumerable<Guid> ids)
    {
        var entities = _dbSet.Where(x => ids.Contains(x.Id));
        await _dbSet.BulkDeleteAsync(entities);
        await _context.BulkSaveChangesAsync();
    }

    public async Task BulkUpdateAsync(IEnumerable<TEntity> entities)
    {
        await _dbSet.BulkUpdateAsync(entities);
        await _context.BulkSaveChangesAsync();
    }

    public async Task BulkSaveChange()
    {
        await _context.BulkSaveChangesAsync();
    }
}