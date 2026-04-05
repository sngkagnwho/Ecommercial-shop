namespace mtkpm.Admin.Infrastructure.Caching
{
    /// <summary>
    /// Interface for caching service
    /// </summary>
    public interface ICacheService
    {
        /// <summary>
        /// Get cached value
        /// </summary>
        T? Get<T>(string key) where T : class;

        /// <summary>
        /// Set cache value
        /// </summary>
        void Set<T>(string key, T value, TimeSpan? expiration = null) where T : class;

        /// <summary>
        /// Remove cache value
        /// </summary>
        void Remove(string key);

        /// <summary>
        /// Remove all cache values by pattern
        /// </summary>
        void RemoveByPattern(string pattern);

        /// <summary>
        /// Check if key exists in cache
        /// </summary>
        bool Exists(string key);
    }
}
