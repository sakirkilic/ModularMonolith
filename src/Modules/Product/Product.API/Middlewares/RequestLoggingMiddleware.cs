using System.Diagnostics;

namespace Product.API.Middlewares
{
    // Gelen HTTP isteklerini loglayan middleware
    public sealed class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(
            RequestDelegate next,
            ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        // Request süresini ve sonucunu loglar
        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            var method = context.Request.Method;
            var path = context.Request.Path;

            _logger.LogInformation(
                "HTTP isteği başladı. Method: {Method}, Path: {Path}",
                method,
                path);

            await _next(context);

            stopwatch.Stop();

            var statusCode = context.Response.StatusCode;
            var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

            _logger.LogInformation(
                "HTTP isteği tamamlandı. Method: {Method}, Path: {Path}, StatusCode: {StatusCode}, Süre: {ElapsedMilliseconds} ms",
                method,
                path,
                statusCode,
                elapsedMilliseconds);
        }
    }
}
