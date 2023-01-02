using System;
namespace MarcoWillems.Template.MinimalMicroservice.Contracts.Caching;

public interface ICacheManager
{
    /// <summary>
    /// Invalidate the cached key
    /// </summary>
    /// <param name="key"></param>
    /// <returns>Returns if the key is deleted</returns>
    Task<bool> InvalidateCacheAsync(string key);
    
    /// <summary>
    /// Invalidate the cached tag
    /// </summary>
    /// <param name="tag"></param>
    /// <returns>Returns how many items are deleted</returns>
    Task<long> InvalidateCacheByTagAsync(string tag);
    
    /// <summary>
    /// Gets the value from redis by key
    /// </summary>
    /// <param name="key"></param>
    /// <returns>The stored value, returns null of cannot be found/expired</returns>
    Task<T?> GetAsync<T>(string key);

    Task<byte[]?> GetAsync(string key);
    /// <summary>
    /// Stores the given value at the given key
    /// </summary>
    /// <param name="key">Redis key</param>
    /// <param name="value">Value</param>
    /// <param name="timeout">When the key expires, if not given it falls back to the default timeoout</param>
    /// <returns>Returns if the value is stored at the specified key</returns>
    Task<bool> SetAsync<T>(string key, T value, string[]? tags = null, TimeSpan? timeout = null);
}