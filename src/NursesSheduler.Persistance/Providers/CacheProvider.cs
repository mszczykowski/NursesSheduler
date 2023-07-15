using Microsoft.Extensions.Caching.Memory;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure.Providers;
using NursesScheduler.BusinessLogic.Exceptions;

namespace NursesScheduler.Infrastructure.Providers
{

    public abstract class CacheProvider<TObject, TKey> : ICacheProvider<TObject, TKey> where TObject : class
    {
        private readonly string _cacheName;


        private readonly IMemoryCache _memoryCache;

        public CacheProvider(IMemoryCache memoryCache, string cacheName)
        {
            _memoryCache = memoryCache;
            _cacheName = cacheName;
        }

        public async Task<TObject> GetCachedDataAsync(TKey key)
        {
            TObject result;
            var cacheKey = BuildCacheKey(key);

            if (!_memoryCache.TryGetValue(cacheKey, out result))
            {
                result = await GetDataFromSource(key)
                    ?? throw new EntityNotFoundException(cacheKey.ToString(), nameof(TObject));

                _memoryCache.Set(cacheKey, result);
            }
            return result;
        }

        public void InvalidateCache(TKey key)
        {
            var cacheKey = BuildCacheKey(key);

            if (_memoryCache.TryGetValue(cacheKey, out var result))
            {
                _memoryCache.Remove(cacheKey);
            }
        }

        protected abstract Task<TObject?> GetDataFromSource(TKey key);

        private string BuildCacheKey(TKey key)
        {
            return $"{_cacheName}-{key}";
        }
    }
}
