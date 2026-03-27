using BuildingBlocks.Domain;
using Product.Domain.Errors;
using Product.Domain.Events;

namespace Product.Domain.Entities
{
    // Ürün aggregate root entity'si
    public sealed class Product : AuditableEntity, IAggregateRoot
    {
        // Ürün adı
        public string Name { get; private set; }

        // Ürün fiyatı
        public decimal Price { get; private set; }

        // Stok miktarı
        public int StockQuantity { get; private set; }

        // EF Core için boş constructor
        private Product()
        {
            Name = string.Empty;
        }

        // Entity oluşturmak için private constructor
        private Product(Guid id, string name, decimal price, int stockQuantity)
        {
            Id = id;
            Name = name;
            Price = price;
            StockQuantity = stockQuantity;
        }

        // Yeni ürün oluşturur
        public static Result<Product> Create(string name, decimal price, int stockQuantity)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return Result<Product>.Failure(ProductErrors.NameEmpty);
            }

            if (price <= 0)
            {
                return Result<Product>.Failure(ProductErrors.PriceMustBeGreaterThanZero);
            }

            if (stockQuantity < 0)
            {
                return Result<Product>.Failure(ProductErrors.StockCannotBeNegative);
            }

            var product = new Product(
                Guid.NewGuid(),
                name.Trim(),
                price,
                stockQuantity);

            product.AddDomainEvent(new ProductCreatedDomainEvent(product.Id));

            return Result<Product>.Success(product);
        }

        // Ürün adını günceller
        public Result ChangeName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return Result.Failure(ProductErrors.NameEmpty);
            }

            Name = name.Trim();
            return Result.Success();
        }

        // Ürün fiyatını günceller
        public Result ChangePrice(decimal price)
        {
            if (price <= 0)
            {
                return Result.Failure(ProductErrors.PriceMustBeGreaterThanZero);
            }

            Price = price;
            return Result.Success();
        }

        // Stok miktarını günceller
        public Result UpdateStock(int stockQuantity)
        {
            if (stockQuantity < 0)
            {
                return Result.Failure(ProductErrors.StockCannotBeNegative);
            }

            StockQuantity = stockQuantity;
            return Result.Success();
        }

        // Ürünü silinmiş olarak işaretler
        public void Delete()
        {
            MarkAsDeleted(DateTime.UtcNow);
        }
    }
}
