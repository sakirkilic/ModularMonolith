using Microsoft.EntityFrameworkCore;
using Product.Application.Abstractions.Data;

namespace Product.Infrastructure.Persistence.Repositories
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


        // Id'ye göre ürünü veritabanından getirir
        public async Task<Product.Domain.Entities.Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }


        // Bekleyen değişiklikleri veritabanına yazar
        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
