using FluentValidation;
using mtkpm.Application.Common.DTOs.Common;
using System.Net;
using System.Text.Json;

namespace mtkpm.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError(exception, "An unhandled exception occurred: {Message}", exception.Message);

            context.Response.ContentType = "application/json";

            var response = exception switch
            {
                ValidationException validationException => new
                {
                    statusCode = (int)HttpStatusCode.BadRequest,
                    response = ApiResponse<object>.FailureResponse(
                        "Validation failed",
                        validationException.Errors.Select(e => e.ErrorMessage).ToList()
                    )
                },
                KeyNotFoundException => new
                {
                    statusCode = (int)HttpStatusCode.NotFound,
                    response = ApiResponse<object>.FailureResponse(exception.Message)
                },
                UnauthorizedAccessException => new
                {
                    statusCode = (int)HttpStatusCode.Forbidden,
                    response = ApiResponse<object>.FailureResponse(exception.Message)
                },
                InvalidOperationException => new
                {
                    statusCode = (int)HttpStatusCode.BadRequest,
                    response = ApiResponse<object>.FailureResponse(exception.Message)
                },
                ArgumentException => new
                {
                    statusCode = (int)HttpStatusCode.BadRequest,
                    response = ApiResponse<object>.FailureResponse(exception.Message)
                },
                _ => new
                {
                    statusCode = (int)HttpStatusCode.InternalServerError,
                    response = ApiResponse<object>.FailureResponse("Đã xảy ra lỗi không mong muốn")
                }
            };

            context.Response.StatusCode = response.statusCode;

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response.response, options));
        }
    }

    public static class ExceptionHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
