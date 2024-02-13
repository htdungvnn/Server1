using System.Linq.Expressions;
using Core.Entity;
using Microsoft.EntityFrameworkCore;

namespace Core.EFRepository;

public class EFRepository<TEntity> : IEFRepository<TEntity> where TEntity : BaseEntity
{
    private readonly DbContext _context;
    private readonly DbSet<TEntity> _dbSet;

    public EFRepository(DbContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    public async Task<TEntity?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    public async Task AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task AddRangeAsync(IEnumerable<TEntity> entities)
    {
        await _dbSet.AddRangeAsync(entities);
        await _context.SaveChangesAsync();
    }

    public async Task Remove(Guid id)
    {
        var entity = await _dbSet.FirstOrDefaultAsync(x => id == x.Id);
        if (entity != null) _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveRange(IEnumerable<Guid> ids)
    {
        var entities = await FindAsync(x => ids.Contains(x.Id));
        var baseEntities = entities as TEntity[] ?? entities.ToArray();
        if (baseEntities.Any()) _dbSet.RemoveRange(baseEntities);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbSet.AnyAsync(predicate);
    }

    public async Task UpdateAsync(TEntity entity)
    {
        _context.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task SaveChange()
    {
        await _context.SaveChangesAsync();
    }
}