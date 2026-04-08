using MediatR;
using Product.Application.Abstractions.Caching;

namespace Product.Application.Behaviors
{
    // Cache'lenebilir query'ler için önbellek davranışı uygular
    public sealed class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly ICacheService _cacheService;

        public CachingBehavior(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        // Cache uygunsa önce cache kontrolü yapar, yoksa handler sonucunu cache'e yazar
        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (request is not ICacheableQuery cacheableQuery)
            {
                return await next();
            }

            var cachedResponse = await _cacheService.GetAsync<TResponse>(cacheableQuery.CacheKey);

            if (cachedResponse is not null)
            {
                return cachedResponse;
            }

            var response = await next();

            await _cacheService.SetAsync(cacheableQuery.CacheKey, response, cacheableQuery.Expiration);

            return response;
        }
    }
}
