namespace mtkpm.Admin.Infrastructure.Http
{
    /// <summary>
    /// Interface for HTTP client wrapper with retry and logging
    /// </summary>
    public interface IHttpClientWrapper
    {
        /// <summary>
        /// Send GET request with retry logic
        /// </summary>
        Task<T?> GetAsync<T>(string endpoint) where T : class;

        /// <summary>
        /// Send POST request with retry logic
        /// </summary>
        Task<T?> PostAsync<T>(string endpoint, object? data = null) where T : class;

        /// <summary>
        /// Send PUT request with retry logic
        /// </summary>
        Task<T?> PutAsync<T>(string endpoint, object? data = null) where T : class;

        /// <summary>
        /// Send DELETE request with retry logic
        /// </summary>
        Task<bool> DeleteAsync(string endpoint);

        /// <summary>
        /// Set authorization header
        /// </summary>
        void SetAuthorizationHeader(string? token);

        /// <summary>
        /// Clear authorization header
        /// </summary>
        void ClearAuthorizationHeader();
    }
}
