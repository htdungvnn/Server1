using System.Data;
using AutoMapper;
using Core.Entity;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Core.DapperRepository;

public class DapperRepository<TEntity> : IDapperRepository<TEntity> where TEntity : BaseEntity
{
    private readonly IConfiguration _config;
    private readonly IDbConnection _db;
    private readonly DapperExtention<TEntity> _extention;
    private readonly IMapper _mapper;
    private readonly Type _table;

    public DapperRepository(IMapper mapper)
    {
        _db = new SqlConnection(_config.GetConnectionString("MoviesDb"));
        _table = typeof(TEntity);
        _mapper = mapper;
    }

    public async Task<TEntity?> GetByIdAsync(Guid id)
    {
        var result = await _db.QueryAsync($"SELECT * FROM {_table.Name+'s'} WHERE Id = @Id", new { Id = id });
        return result.FirstOrDefault();
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        var sql = "SELECT * FROM " + _table.Name+'s';
        var result = _db.QueryAsync(sql);
        return _mapper.Map<IEnumerable<TEntity>>(result);
    }

    public async Task AddAsync(TEntity entity)
    {
        var propGet = _extention.GetPropName(DapperExtention<TEntity>.ActionExcute.Get);
        var propAdd = _extention.GetPropName(DapperExtention<TEntity>.ActionExcute.Add);
        var sql = $"INSERT INTO {_table.Name+'s'}({propGet}) VALUES ({propAdd})";
        await _db.ExecuteAsync(sql, entity);
    }

    public async Task Remove(Guid id)
    {
        await _db.ExecuteAsync($"DELETE FROM {_table.Name+'s'} WHERE Id=@Id", new { Id = id });
    }

    public async Task UpdateAsync(TEntity entity)
    {
        var propUpdate = _extention.GetPropName(DapperExtention<TEntity>.ActionExcute.Update);
        await _db.ExecuteAsync($"UPDATE {_table.Name+'s'} SET +{propUpdate} WHERE Id=@Id", entity);
    }
    // private enum ActionExcute
    // {
    //     Add,
    //     Get,
    //     Update
    // }
    //
    // private string GetPropName(ActionExcute action)
    // {
    //     var sql = "";
    //     switch (action)
    //     {
    //         case ActionExcute.Add:
    //             foreach (var propertyInfo in _table.GetProperties())
    //             {
    //                 sql +='@'+ propertyInfo.Name + ',';
    //             }
    //             break;
    //         case ActionExcute.Get:
    //             foreach (var propertyInfo in _table.GetProperties())
    //             {
    //                 sql += propertyInfo.Name + ',';
    //             }
    //             break;
    //         case ActionExcute.Update:
    //             foreach (var propertyInfo in _table.GetProperties())
    //             {
    //                 sql += propertyInfo.Name + "= @"+propertyInfo.Name +',';
    //             }
    //             break;
    //             default: break;
    //             
    //     }
    //     return sql;
    // }
}