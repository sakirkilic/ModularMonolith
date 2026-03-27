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

        // Sayfalı ürün listesini veritabanından getirir
        public async Task<(List<Product.Domain.Entities.Product> Items, int TotalCount)> GetPagedAsync(
            int page,
            int pageSize,
            string? search,
            decimal? minPrice,
            decimal? maxPrice,
            CancellationToken cancellationToken)
        {
            IQueryable<Product.Domain.Entities.Product> query = _dbContext.Products.AsNoTracking();

            // 🔍 SEARCH
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x => x.Name.Contains(search));
            }

            // 💰 MIN PRICE
            if (minPrice.HasValue)
            {
                query = query.Where(x => x.Price >= minPrice.Value);
            }

            // 💰 MAX PRICE
            if (maxPrice.HasValue)
            {
                query = query.Where(x => x.Price <= maxPrice.Value);
            }

            // sıralama
            query = query.OrderBy(x => x.Name);

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }



        // Bekleyen değişiklikleri veritabanına yazar
        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
