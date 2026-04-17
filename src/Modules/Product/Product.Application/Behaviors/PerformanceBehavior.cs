using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Product.Application.Behaviors
{
    // Request sürelerini ölçen behavior
    public sealed class PerformanceBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<PerformanceBehavior<TRequest, TResponse>> _logger;

        public PerformanceBehavior(ILogger<PerformanceBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            var stopwatch = Stopwatch.StartNew();

            var response = await next();

            stopwatch.Stop();

            var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

            var requestName = typeof(TRequest).Name;

            // Yavaş istekleri ayrıca logla
            if (elapsedMilliseconds > 500)
            {
                _logger.LogWarning(
                    "YAVAŞ İSTEK TESPİT EDİLDİ: {RequestName} - Süre: {ElapsedMilliseconds} ms",
                    requestName,
                    elapsedMilliseconds);
            }
            else
            {
                _logger.LogInformation(
                    "İstek işlendi: {RequestName} - Süre: {ElapsedMilliseconds} ms",
                    requestName,
                    elapsedMilliseconds);
            }

            return response;
        }
    }
}
