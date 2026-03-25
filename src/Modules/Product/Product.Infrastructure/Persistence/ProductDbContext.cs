using Microsoft.EntityFrameworkCore;

namespace Product.Infrastructure.Persistence
{
    // Product modülünün veritabanı erişim katmanı
    public sealed class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options)
            : base(options)
        {
        }

        // Product tablosunu temsil eder
        public DbSet<Product.Domain.Entities.Product> Products => Set<Product.Domain.Entities.Product>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("product");

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
