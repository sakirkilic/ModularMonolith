namespace Product.Application.Abstractions.Data
{
    // Product verileri için repository sözleşmesi
    public interface IProductRepository
    {
        // Yeni ürünü ekler
        Task AddAsync(Product.Domain.Entities.Product product, CancellationToken cancellationToken);

        // Id'ye göre ürünü getirir
        Task<Product.Domain.Entities.Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        // tüm ürünleri getirir
        Task<List<Product.Domain.Entities.Product>> GetAllAsync(CancellationToken cancellationToken);

        // Değişiklikleri veritabanına yazar
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
