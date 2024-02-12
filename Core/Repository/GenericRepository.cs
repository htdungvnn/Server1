using System.Data;
using System.Linq.Expressions;
using AutoMapper;
using Core.DapperRepository;
using Core.Entity;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Core.Repository;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
{
    private readonly DbContext _context;
    private readonly IDbConnection _db;
    private readonly DbSet<TEntity> _dbSet;
    private readonly IMapper _mapper;
    private readonly Type _table;

    public GenericRepository(DbContext context, IMapper mapper)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
        _db = new SqlConnection("Server=localhost,1433;Database=MoviesDb;User Id=SA;password=yourStrong(!)Password;TrustServerCertificate=True");
        _mapper = mapper;
        _table = typeof(TEntity);
    }

    public async Task<TEntity?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        var sql = $"SELECT * FROM {_table.Name}s";
        var result = await _db.QueryAsync(sql);
        if (result != null)
        {
            return _mapper.Map<IEnumerable<TEntity>>(result);
        }

        return new List<TEntity>();
    }

    public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    public async Task AddAsync(TEntity entity)
    {
        var propGet = GetPropName(ActionExcute.Get);
        var propAdd = GetPropName(ActionExcute.Add);
        var sql = $"INSERT INTO {_table.Name+'s'}({propGet}) VALUES ({propAdd})";
        await _db.ExecuteAsync(sql, entity);
    }

    public async Task AddRangeAsync(IEnumerable<TEntity> entities)
    {
        await _dbSet.BulkInsertAsync(entities);
        await _context.BulkSaveChangesAsync();
    }

    public async Task Remove(Guid id)
    {
        await _db.ExecuteAsync($"DELETE FROM {_table.Name+'s'} WHERE Id=@Id", new { Id = id });
    }

    public async Task RemoveRange(IEnumerable<Guid> ids)
    {
        var entities = _dbSet.Where(x => ids.Contains(x.Id));
        await _dbSet.BulkDeleteAsync(entities);
        await _context.BulkSaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbSet.AnyAsync(predicate);
    }

    public async Task UpdateAsync(TEntity entity)
    {
        var propUpdate = GetPropName(ActionExcute.Update);
        await _db.ExecuteAsync($"UPDATE {_table.Name+'s'} SET +{propUpdate} WHERE Id=@Id", entity);
    }

    public async Task SaveChange()
    {
        await _context.BulkSaveChangesAsync();
    }
    
    public enum ActionExcute
    {
        Add,
        Get,
        Update
    }
    
    public string GetPropName(ActionExcute action)
    {
        var sql = "";
        switch (action)
        {
            case ActionExcute.Add:
                foreach (var propertyInfo in typeof(TEntity).GetProperties()) sql += '@' + propertyInfo.Name + ',';
                break;
            case ActionExcute.Get:
                foreach (var propertyInfo in typeof(TEntity).GetProperties()) sql += propertyInfo.Name + ',';
                break;
            case ActionExcute.Update:
                foreach (var propertyInfo in typeof(TEntity).GetProperties())
                    sql += propertyInfo.Name + "= @" + propertyInfo.Name + ',';
                break;
        }

        return sql.Remove(sql.Length-1);
    }
}