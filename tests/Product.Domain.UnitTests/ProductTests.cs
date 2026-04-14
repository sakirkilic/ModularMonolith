using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Domain.UnitTests
{
    // Product entity davranışlarını test eder
    public sealed class ProductTests
    {
        [Fact]
        public void Create_Should_ReturnSuccess_When_DataIsValid()
        {
            // Arrange (hazırlık)
            var name = "Laptop";
            var price = 1000m;
            var stockQuantity = 10;

            // Act (aksiyon)
            var result = Product.Domain.Entities.Product.Create(name, price, stockQuantity);

            // Assert (doğrulama)
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(name, result.Value.Name);
            Assert.Equal(price, result.Value.Price);
            Assert.Equal(stockQuantity, result.Value.StockQuantity);
        }

        [Fact]
        public void Create_Should_ReturnFailure_When_NameIsEmpty()
        {
            // Arrange
            var name = "";
            var price = 1000m;
            var stockQuantity = 10;

            // Act
            var result = Product.Domain.Entities.Product.Create(name, price, stockQuantity);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("Ürün adı boş olamaz.", result.Error.Message);
        }

        [Fact]
        public void Create_Should_ReturnFailure_When_PriceIsLessThanOrEqualToZero()
        {
            // Arrange
            var name = "Laptop";
            var price = 0m;
            var stockQuantity = 10;

            // Act
            var result = Product.Domain.Entities.Product.Create(name, price, stockQuantity);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("Ürün fiyatı sıfırdan büyük olmalıdır.", result.Error.Message);
        }

        [Fact]
        public void Create_Should_ReturnFailure_When_StockIsNegative()
        {
            // Arrange
            var name = "Laptop";
            var price = 1000m;
            var stockQuantity = -1;

            // Act
            var result = Product.Domain.Entities.Product.Create(name, price, stockQuantity);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("Stok miktarı negatif olamaz.", result.Error.Message);
        }

        [Fact]
        public void ChangePrice_Should_ReturnFailure_When_PriceIsLessThanOrEqualToZero()
        {
            // Arrange
            var productResult = Product.Domain.Entities.Product.Create("Laptop", 1000m, 10);
            var product = productResult.Value;

            // Act
            var result = product.ChangePrice(0);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("Ürün fiyatı sıfırdan büyük olmalıdır.", result.Error.Message);
        }

        [Fact]
        public void UpdateStock_Should_ReturnFailure_When_StockIsNegative()
        {
            // Arrange
            var productResult = Product.Domain.Entities.Product.Create("Laptop", 1000m, 10);
            var product = productResult.Value;

            // Act
            var result = product.UpdateStock(-5);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("Stok miktarı negatif olamaz.", result.Error.Message);
        }
    }
}
