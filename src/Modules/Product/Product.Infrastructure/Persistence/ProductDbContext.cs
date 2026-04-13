using BuildingBlocks.Domain;
using Microsoft.EntityFrameworkCore;
using Product.Application.Abstractions.Authentication;

namespace Product.Infrastructure.Persistence
{
    // Product modülünün veritabanı erişim katmanı
    public sealed class ProductDbContext : DbContext
    {
        private readonly ICurrentUserService _currentUserService;

        public ProductDbContext(DbContextOptions<ProductDbContext> options, ICurrentUserService currentUserService)
            : base(options)
        {
            _currentUserService = currentUserService;
        }

        // Product tablosunu temsil eder
        public DbSet<Product.Domain.Entities.Product> Products => Set<Product.Domain.Entities.Product>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("product");

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductDbContext).Assembly);

            // Global Query Filter
            modelBuilder.Entity<Product.Domain.Entities.Product>()
                .HasQueryFilter(x => !x.IsDeleted);

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
            var currentUser = _currentUserService.GetCurrentUser();
            var currentUserId = currentUser.UserId;

            var entries = ChangeTracker.Entries<AuditableEntity>();

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.SetCreatedAt(utcNow);
                    entry.Entity.SetCreatedBy(currentUserId);
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.SetUpdatedAt(utcNow);
                    entry.Entity.SetUpdatedBy(currentUserId);
                }
            }
        }
    }
}
