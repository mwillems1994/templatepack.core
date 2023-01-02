using System;
using MarcoWillems.Template.MinimalMicroservice.Contracts.Caching;
using Microsoft.AspNetCore.OutputCaching;

namespace MarcoWillems.Template.MinimalMicroservice
{
	public class RedisOutputCacheStore : IOutputCacheStore
	{
        private readonly ICacheManager _cacheManager;

        public RedisOutputCacheStore(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public async ValueTask EvictByTagAsync(string tag, CancellationToken cancellationToken)
        {
            await _cacheManager.InvalidateCacheByTagAsync(tag);
        }

        public async ValueTask<byte[]?> GetAsync(string key, CancellationToken cancellationToken)
        {
            var value = await _cacheManager.GetAsync(key);

            return value;
        }

        public async ValueTask SetAsync(string key, byte[] value, string[]? tags, TimeSpan validFor, CancellationToken cancellationToken)
        {
            await _cacheManager.SetAsync(key, value, tags, validFor);
        }
    }
}

