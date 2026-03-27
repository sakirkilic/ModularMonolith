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

        // Id'ye göre silinmemiş ürünü veritabanından getirir
        public async Task<Product.Domain.Entities.Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _dbContext.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken);
        }

        // Güncelleme işlemleri için silinmemiş tracked product getirir
        public async Task<Product.Domain.Entities.Product?> GetTrackedByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _dbContext.Products
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken);
        }

        // Sayfalı, filtreli ve sıralı ürün listesini veritabanından getirir
        public async Task<(List<Product.Domain.Entities.Product> Items, int TotalCount)> GetPagedAsync(
            int page,
            int pageSize,
            string? search,
            decimal? minPrice,
            decimal? maxPrice,
            string? sortBy,
            string? sortDirection,
            CancellationToken cancellationToken)
        {
            IQueryable<Product.Domain.Entities.Product> query = _dbContext.Products
                .AsNoTracking()
                .Where(x => !x.IsDeleted);

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x => x.Name.Contains(search));
            }

            if (minPrice.HasValue)
            {
                query = query.Where(x => x.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(x => x.Price <= maxPrice.Value);
            }

            query = ApplySorting(query, sortBy, sortDirection);

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

        // İzin verilen alanlara göre sıralama uygular
        private static IQueryable<Product.Domain.Entities.Product> ApplySorting(
            IQueryable<Product.Domain.Entities.Product> query,
            string? sortBy,
            string? sortDirection)
        {
            var normalizedSortBy = sortBy?.Trim().ToLower();
            var normalizedSortDirection = sortDirection?.Trim().ToLower();

            var isDescending = normalizedSortDirection == "desc";

            return normalizedSortBy switch
            {
                "price" => isDescending
                    ? query.OrderByDescending(x => x.Price)
                    : query.OrderBy(x => x.Price),

                "stockquantity" => isDescending
                    ? query.OrderByDescending(x => x.StockQuantity)
                    : query.OrderBy(x => x.StockQuantity),

                _ => isDescending
                    ? query.OrderByDescending(x => x.Name)
                    : query.OrderBy(x => x.Name)
            };
        }
    }
}
