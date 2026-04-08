namespace Product.Application.Abstractions.Caching
{
    // Cache'lenebilir query'ler için sözleşme
    public interface ICacheableQuery
    {
        // Cache anahtarı
        string CacheKey { get; }

        // Cache süresi (dakika)
        TimeSpan Expiration { get; }
    }
}
