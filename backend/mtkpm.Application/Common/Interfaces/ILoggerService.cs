namespace mtkpm.Application.Common.Interfaces
{
    public interface ILoggerService
    {
        void LogInfo(string message, string category = "General");
        void LogWarning(string message, string category = "General");
        void LogError(string message, string category = "General");
    }
}
