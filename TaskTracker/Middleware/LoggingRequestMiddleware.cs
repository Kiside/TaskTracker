namespace TaskTracker.Middleware
{
    public class LoggingRequestMiddleware
    {
        private readonly ILogger<LoggingRequestMiddleware> _logger;
        private readonly RequestDelegate _next;

        public LoggingRequestMiddleware(ILogger<LoggingRequestMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var request = context.Request;

            _logger.LogInformation($"Incoming Request: {request.Method} {request.Path} from {context.Connection.RemoteIpAddress}");

            await _next(context);

            _logger.LogInformation($"Outgoing Response: {context.Response.StatusCode}");
        }
    }
}
