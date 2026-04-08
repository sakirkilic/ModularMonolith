using Microsoft.Extensions.Caching.Memory;
using Product.Application.Abstractions.Caching;

namespace Product.Infrastructure.Caching
{
    // Memory cache implementasyonu (in-memory önbellek)
    public sealed class MemoryCacheService : ICacheService
    {
        private readonly IMemoryCache _cache;

        public MemoryCacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        // Cache'ten veri alır
        public Task<T?> GetAsync<T>(string key)
        {
            _cache.TryGetValue(key, out T? value);
            return Task.FromResult(value);
        }

        // Cache'e veri yazar
        public Task SetAsync<T>(string key, T value, TimeSpan expiration)
        {
            _cache.Set(key, value, expiration);
            return Task.CompletedTask;
        }

        // Cache'ten veri siler
        public Task RemoveAsync(string key)
        {
            _cache.Remove(key);
            return Task.CompletedTask;
        }
    }
}
