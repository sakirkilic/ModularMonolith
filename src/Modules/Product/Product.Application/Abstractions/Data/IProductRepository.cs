namespace Product.Application.Abstractions.Data
{
    // Product verileri için repository sözleşmesi
    public interface IProductRepository
    {
        // Yeni ürünü ekler
        Task AddAsync(Product.Domain.Entities.Product product, CancellationToken cancellationToken);

        // Id'ye göre ürünü getirir
        Task<Product.Domain.Entities.Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        // Güncelleme işlemleri için tracked product getirir
        Task<Product.Domain.Entities.Product?> GetTrackedByIdAsync(Guid id, CancellationToken cancellationToken);

        // Sayfalı ürün listesini getirir
        Task<(List<Product.Domain.Entities.Product> Items, int TotalCount)> GetPagedAsync(
            int page,
            int pageSize,
            string? search,
            decimal? minPrice,
            decimal? maxPrice,
            string? sortBy,
            string? sortDirection,
            CancellationToken cancellationToken);

        // Değişiklikleri veritabanına yazar
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
