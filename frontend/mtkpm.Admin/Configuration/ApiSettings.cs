namespace mtkpm.Admin.Configuration
{
    /// <summary>
    /// API backend configuration settings
    /// </summary>
    public class ApiSettings
    {
        public string BaseUrl { get; set; } = "https://localhost:5001/api";
        public int RequestTimeoutSeconds { get; set; } = 30;
        public string ApiVersion { get; set; } = "v1";
    }
}
