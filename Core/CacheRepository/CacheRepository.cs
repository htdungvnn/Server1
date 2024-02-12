using Core.Entity;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Core.CacheRepository;

public class CacheRepository<TEntity> : ICacheRepository<TEntity> where TEntity : BaseEntity
{
    private readonly IDatabase _database;
    private readonly ConnectionMultiplexer _redisConnection;

    public CacheRepository(string connectionString)
    {
        _redisConnection = ConnectionMultiplexer.Connect(connectionString);
        _database = _redisConnection.GetDatabase();
    }

    public async Task<TEntity> GetAsync(Guid id)
    {
        var value = await _database.StringGetAsync(id.ToString());

        return JsonConvert.DeserializeObject<TEntity>(value);
    }

    public async Task SetAsync(Guid id, TEntity entity, TimeSpan? expiry = null)
    {
        var serializedValue = JsonConvert.SerializeObject(entity);
        await _database.StringSetAsync(id.ToString(), serializedValue, expiry);
    }

    public async Task<bool> RemoveAsync(Guid id)
    {
        return await _database.KeyDeleteAsync(id.ToString());
    }
}