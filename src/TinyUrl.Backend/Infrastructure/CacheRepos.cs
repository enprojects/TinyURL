using Microsoft.Extensions.Logging;
using TinyUrl.Backend.Configurations;
using System.Collections.Concurrent;

namespace TinyUrl.Backend.Infrastructure
{
    public class CacheRepos : ICacheRepos
    {
        private readonly ILogger<CacheRepos> _logger;
        private readonly CacheConfiguration _cacheConfiguration;
        private readonly IDbContext _dbContext;
        private readonly ConcurrentDictionary<string, CacheItem> _inMemoryCache;

        public CacheRepos(ILogger<CacheRepos> logger, 
                         CacheConfiguration cacheConfiguration)
        {
            _logger = logger;
            _cacheConfiguration = cacheConfiguration;
            _inMemoryCache = new();
        }

        // I didn't load all cache from db because db limitation, and unknown  db size 
        public ValueTask<CacheItem?> GetCacheItem(string key)
        {
            try
            {
                if (_inMemoryCache.TryGetValue(key, out var cacheItem))
                {
                    var updatedCacheItem = new CacheItem( DateTime.UtcNow, cacheItem.Value);
                    _inMemoryCache.AddOrUpdate(key, updatedCacheItem, (existingKey, existingValue) => updatedCacheItem);
                }
                return new ValueTask<CacheItem?>(cacheItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while trying to retrieve from {key}");
            }

            return default;
        }

        public async Task SetCacheItem(string key, byte[] value)
        {
            _inMemoryCache.AddOrUpdate(key,
                addValueFactory: k => new CacheItem(DateTime.Now,value),
                updateValueFactory: (k, existing) =>
                {
                    existing.LastAccessed = DateTime.Now;
                    return existing;
                });

            if (_inMemoryCache.Count > _cacheConfiguration.MaxItems)
            {
               await CacheEvictionPolicy();
            }
        }
        /// <summary>
        /// This like lRU base,  eviction does not happen with every new record insertion,
        /// but rather based on the cache reaching its capacity limit
        /// </summary>
        /// <returns></returns>


        private Task CacheEvictionPolicy()
        {
            var itemsToBeRemoved = _inMemoryCache.Keys.OrderBy(k => _inMemoryCache[k].LastAccessed)
                .Take(_inMemoryCache.Count - _cacheConfiguration.MaxItems);

            foreach (var key in itemsToBeRemoved)
            {
                _inMemoryCache.TryRemove(key, out var _);
            }
            return Task.CompletedTask;
        }

    }
}
