using HouseBroker.Application.Interfaces.IServices;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace HouseBroker.Infrastructure.Services;

public class RedisCacheService(IDistributedCache distributedCache) : ICacheService
{
    private readonly IDistributedCache _distributedCache = distributedCache ?? throw new ArgumentNullException(nameof(distributedCache));

    public async Task<T?> GetAsync<T>(string key)
    {
        var cachedData = await _distributedCache.GetStringAsync(key);
        return cachedData is null ? default : JsonSerializer.Deserialize<T>(cachedData);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpireTime = null, TimeSpan? slidingExpireTime = null)
    {
        var options = new DistributedCacheEntryOptions();

        if (absoluteExpireTime.HasValue)
        {
            options.SetAbsoluteExpiration(absoluteExpireTime.Value);
        }

        if (slidingExpireTime.HasValue)
        {
            options.SetSlidingExpiration(slidingExpireTime.Value);
        }

        var serializedData = JsonSerializer.Serialize(value);
        await _distributedCache.SetStringAsync(key, serializedData, options);
    }

    public async Task RemoveAsync(string key)
    {
        await _distributedCache.RemoveAsync(key);
    }

    public async Task<int> GetOrSetVersionAsync(string versionKey)
    {
        var version = await GetAsync<int>(versionKey);
        if (version != 0) return version; // if version exist 
        version = 1;
        await SetAsync(versionKey, version, TimeSpan.FromDays(7));
        return version;
    }

    public async Task IncrementVersionAsync(string versionKey)
    {
        var current = await GetOrSetVersionAsync(versionKey);
        await SetAsync(versionKey, current + 1, TimeSpan.FromDays(7));
    }
}
