namespace Product.Application.Abstractions.Caching
{
    // Cache işlemleri için sözleşme (soyutlama)
    public interface ICacheService
    {
        Task<T?> GetAsync<T>(string key);

        Task SetAsync<T>(string key, T value, TimeSpan expiration);

        Task RemoveAsync(string key);
    }
}
