using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Product.Infrastructure.Persistence.Configurations
{
    // Product entity'sinin veritabanı eşlemesini yapar
    public sealed class ProductConfiguration : IEntityTypeConfiguration<Product.Domain.Entities.Product>
    {
        public void Configure(EntityTypeBuilder<Product.Domain.Entities.Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Price)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.StockQuantity)
                .IsRequired();
        }
    }
}
