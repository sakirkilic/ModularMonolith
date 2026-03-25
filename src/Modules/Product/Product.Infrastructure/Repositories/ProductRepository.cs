using Product.Application.Abstractions.Data;

namespace Product.Infrastructure.Repositories
{
    // Product repository sözleşmesinin EF Core implementasyonu
    public sealed class ProductRepository : IProductRepository
    {
        private readonly ProductDbContext _dbContext;

        public ProductRepository(ProductDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Yeni ürünü context'e ekler
        public async Task AddAsync(Product.Domain.Entities.Product product, CancellationToken cancellationToken)
        {
            await _dbContext.Products.AddAsync(product, cancellationToken);
        }

        // Bekleyen değişiklikleri veritabanına yazar
        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
