using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Threading.Tasks;
using mtkpm.Infrastructure.Services;
using mtkpm.Application.Common.Interfaces;

namespace mtkpm.Middleware
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerService _logger;

        public RequestResponseLoggingMiddleware(RequestDelegate next, ILoggerService logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            var request = context.Request;
            var user = context.User?.Identity?.Name ?? "Anonymous";
            _logger.LogInfo($"[Request] {request.Method} {request.Path} by {user}", "API");

            await _next(context);

            stopwatch.Stop();
            var response = context.Response;
            _logger.LogInfo($"[Response] {request.Method} {request.Path} - {response.StatusCode} ({stopwatch.ElapsedMilliseconds} ms)", "API");
        }
    }
}