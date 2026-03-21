using mtkpm.Application.Common.Interfaces;
using System;

namespace mtkpm.Infrastructure.Services
{
    public sealed class LoggerService : ILoggerService
    {
        private static readonly Lazy<LoggerService> _instance = new(() => new LoggerService());
        public static LoggerService Instance => _instance.Value;

        // Private constructor to prevent instantiation
        private LoggerService() { }

        public void LogInfo(string message, string category = "General")
        {
            Console.WriteLine($"[INFO][{category}] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
        }

        public void LogWarning(string message, string category = "General")
        {
            Console.WriteLine($"[WARN][{category}] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
        }

        public void LogError(string message, string category = "General")
        {
            Console.WriteLine($"[ERROR][{category}] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
        }
    }
}