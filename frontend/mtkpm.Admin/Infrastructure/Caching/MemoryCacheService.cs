using Microsoft.Extensions.Caching.Memory;

namespace mtkpm.Admin.Infrastructure.Caching
{
    /// <summary>
    /// In-memory cache service implementation
    /// </summary>
    public class MemoryCacheService : ICacheService
    {
        private readonly IMemoryCache _cache;
        private readonly ILogger<MemoryCacheService> _logger;

        public MemoryCacheService(IMemoryCache cache, ILogger<MemoryCacheService> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public T? Get<T>(string key) where T : class
        {
            if (_cache.TryGetValue(key, out T? value))
            {
                _logger.LogInformation($"Cache hit: {key}");
                return value;
            }
            _logger.LogInformation($"Cache miss: {key}");
            return null;
        }

        public void Set<T>(string key, T value, TimeSpan? expiration = null) where T : class
        {
            var cacheOptions = new MemoryCacheEntryOptions();
            if (expiration.HasValue)
            {
                cacheOptions.AbsoluteExpirationRelativeToNow = expiration;
            }
            else
            {
                cacheOptions.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
            }

            _cache.Set(key, value, cacheOptions);
            _logger.LogInformation($"Cache set: {key} (expires in {expiration?.TotalMinutes} minutes)");
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
            _logger.LogInformation($"Cache removed: {key}");
        }

        public void RemoveByPattern(string pattern)
        {
            // IMemoryCache doesn't support pattern removal directly
            // This would need to be handled differently (e.g., keeping a list of keys)
            _logger.LogWarning($"RemoveByPattern not fully supported in MemoryCache: {pattern}");
        }

        public bool Exists(string key)
        {
            return _cache.TryGetValue(key, out _);
        }
    }
}
