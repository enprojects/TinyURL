namespace TinyUrl.Backend.Infrastructure
{
    public interface ICacheRepos
    {
        ValueTask<CacheItem?> GetCacheItem(string key);
        Task SetCacheItem(string key, byte[] value);
    }
}