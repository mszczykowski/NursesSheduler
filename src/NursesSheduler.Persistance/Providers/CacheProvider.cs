using Microsoft.Extensions.Caching.Memory;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure.Providers;
using NursesScheduler.BusinessLogic.Exceptions;

namespace NursesScheduler.Infrastructure.Providers
{

    public abstract class CacheProvider<TObject, TKey> : ICacheProvider<TObject, TKey> where TObject : class
    {
        private readonly string _cacheKey;


        private readonly IMemoryCache _memoryCache;

        public CacheProvider(IMemoryCache memoryCache, string cacheKey)
        {
            _memoryCache = memoryCache;
            _cacheKey = cacheKey;
        }

        public async Task<TObject> GetCachedDataAsync(TKey id)
        {
            TObject result;
            var key = BuildKey(id);

            if (!_memoryCache.TryGetValue(key, out result))
            {
                result = await GetDataFromSource(id)
                    ?? throw new EntityNotFoundException(id.ToString(), nameof(TObject));

                _memoryCache.Set(key, result);
            }
            return result;
        }

        public void InvalidateCache(TKey id)
        {
            var key = BuildKey(id);

            if (_memoryCache.TryGetValue(key, out var result))
            {
                _memoryCache.Remove(key);
            }
        }

        protected abstract Task<TObject?> GetDataFromSource(TKey id);

        private string BuildKey(TKey id)
        {
            return $"{_cacheKey}-{id}";
        }
    }
}
