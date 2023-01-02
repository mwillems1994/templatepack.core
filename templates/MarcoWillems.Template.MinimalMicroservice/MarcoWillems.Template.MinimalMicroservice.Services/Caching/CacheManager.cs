using MarcoWillems.Template.MinimalMicroservice.Contracts.Caching;
using MarcoWillems.Template.MinimalMicroservice.Contracts.Options;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace MarcoWillems.Template.MinimalMicroservice.Services.Caching;

public class CacheManager : ICacheManager
{
    private readonly CachingOptions _cachingOptions;
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    public CacheManager(IOptions<CachingOptions> cachingOptions)
    {
        if(cachingOptions == null)
        {
            throw new ArgumentNullException(nameof(cachingOptions));
        }
        _cachingOptions = cachingOptions.Value;
        _connectionMultiplexer = ConnectionMultiplexer.Connect(_cachingOptions.ConnectionString, opt =>
        {
            opt.Password = _cachingOptions.Password;
        });
    }

    public async Task<bool> InvalidateCacheAsync(string key)
    {
        var db = _connectionMultiplexer.GetDatabase();

        var deleted = await db.KeyDeleteAsync(key);

        return deleted;
    }

    public async Task<long> InvalidateCacheByTagAsync(string tag)
    {
        var db = _connectionMultiplexer.GetDatabase();

        var memberKeys = db.SetMembers($"tag_{tag}").Select(x => x.ToString());
        var redisKeys = memberKeys.Select(x => new RedisKey(x)).ToArray();
        var deleted = await db.KeyDeleteAsync(redisKeys);

        return deleted;
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var db = _connectionMultiplexer.GetDatabase();
        var redisValue = await db.StringGetAsync(key);

        if (redisValue.IsNullOrEmpty)
        {
            return default;
        }

        var value = JsonConvert.DeserializeObject<T>(redisValue!);

        return value;
    }

    public async Task<byte[]?> GetAsync(string key)
    {
        var db = _connectionMultiplexer.GetDatabase();
        var redisValue = await db.StringGetAsync(key);

        return redisValue;
    }

    public async Task<bool> SetAsync<T>(string key, T value, string[]? tags = null, TimeSpan? timeout = null)
    {
        var db = _connectionMultiplexer.GetDatabase();

        var serializedJson = JsonConvert.SerializeObject(value);

        var isSet = await db.StringSetAsync(key, serializedJson, timeout ?? _cachingOptions.DefaultTimeout);

        await AddKeyToTagSet(db, key, tags);

        return isSet;
    }
    
    private static async Task AddKeyToTagSet(IDatabaseAsync db, string key, string[]? tags)
    {
        if (tags == null)
        {
            return;
        }

        await Task.WhenAll(tags.Select(async tag =>
        {
            await db.SetAddAsync($"tag_{tag}", key);
        }));
    }
}