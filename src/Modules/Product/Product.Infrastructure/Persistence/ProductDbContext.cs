using BuildingBlocks.Domain;
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

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyAuditInformation();

            return await base.SaveChangesAsync(cancellationToken);
        }

        // Audit alanlarını entity state'e göre doldurur
        private void ApplyAuditInformation()
        {
            var utcNow = DateTime.UtcNow;

            var entries = ChangeTracker.Entries<AuditableEntity>();

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.SetCreatedAt(utcNow);
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.SetUpdatedAt(utcNow);
                }
            }
        }
    }
}
