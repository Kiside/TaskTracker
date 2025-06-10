using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using TaskTracker.Common;
using TaskTracker.Exceptions;

namespace TaskTracker.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception caught");

                context.Response.ContentType = "application/json";

                var sCode = ex switch
                {
                    KeyNotFoundException => StatusCodes.Status404NotFound,
                    UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                    ValidationException => StatusCodes.Status400BadRequest,
                    NullReferenceException => StatusCodes.Status500InternalServerError,
                    BusinessRuleException => StatusCodes.Status422UnprocessableEntity,
                    _ => StatusCodes.Status500InternalServerError
                };

                var apiResponse =  ApiResponse<string>.Response(
                    data: null,
                    statusCode: (HttpStatusCode) sCode,
                    message: ex.Message
                    );

                var json = JsonSerializer.Serialize(apiResponse);

                await context.Response.WriteAsJsonAsync(json);
            }

        }
    }
}
