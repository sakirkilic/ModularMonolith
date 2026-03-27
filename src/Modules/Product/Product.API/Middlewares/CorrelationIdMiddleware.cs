using Serilog.Context;

namespace Product.API.Middlewares
{
    // Her request için correlation id üretir veya var olanı kullanır
    public sealed class CorrelationIdMiddleware
    {
        private const string CorrelationIdHeaderName = "X-Correlation-Id";

        private readonly RequestDelegate _next;

        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        // Correlation id'yi request ve response akışına ekler
        public async Task InvokeAsync(HttpContext context)
        {
            var correlationId = GetOrCreateCorrelationId(context);

            context.Response.Headers[CorrelationIdHeaderName] = correlationId;

            using (LogContext.PushProperty("CorrelationId", correlationId))
            {
                await _next(context);
            }
        }

        // Header'dan correlation id alır, yoksa yenisini üretir
        private static string GetOrCreateCorrelationId(HttpContext context)
        {
            var correlationId = context.Request.Headers[CorrelationIdHeaderName].FirstOrDefault();

            return string.IsNullOrWhiteSpace(correlationId)
                ? Guid.NewGuid().ToString()
                : correlationId;
        }
    }
}
