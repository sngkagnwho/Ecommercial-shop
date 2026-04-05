namespace mtkpm.Admin.Infrastructure.Http
{
    /// <summary>
    /// Configuration for HTTP client
    /// </summary>
    public class HttpClientConfiguration
    {
        public string? BaseUrl { get; set; }
        public int TimeoutSeconds { get; set; } = 30;
        public int MaxRetries { get; set; } = 3;
        public bool EnableLogging { get; set; } = true;
    }
}
