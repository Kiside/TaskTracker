using System.ComponentModel.DataAnnotations;
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

                context.Response.StatusCode = ex switch
                {
                    KeyNotFoundException => StatusCodes.Status404NotFound,
                    UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                    ValidationException => StatusCodes.Status400BadRequest,
                    NullReferenceException => StatusCodes.Status500InternalServerError,
                    BusinessRuleException => StatusCodes.Status422UnprocessableEntity,
                    _ => StatusCodes.Status500InternalServerError
                };

                var response = new
                {
                    error = ex.Message,
                    type = ex.GetType().Name,
                    status = context.Response.StatusCode,
                };

                await context.Response.WriteAsJsonAsync(response);
            }

        }
    }
}
