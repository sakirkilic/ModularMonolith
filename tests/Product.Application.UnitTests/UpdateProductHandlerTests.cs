using BuildingBlocks.Domain.Exceptions;
using Moq;
using Product.Application.Abstractions.Caching;
using Product.Application.Abstractions.Data;
using Product.Application.Features.Products.UpdateProduct;

namespace Product.Application.UnitTests
{
    // UpdateProductHandler davranışlarını test eder
    public sealed class UpdateProductHandlerTests
    {
        [Fact]
        public async Task Handle_Should_UpdateProduct_When_ProductExists()
        {
            // Arrange (hazırlık)
            var productResult = Product.Domain.Entities.Product.Create("Eski Ürün", 100m, 5);
            var product = productResult.Value;

            var repositoryMock = new Mock<IProductRepository>();
            var cacheServiceMock = new Mock<ICacheService>();

            repositoryMock
                .Setup(x => x.GetTrackedByIdAsync(product.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            var handler = new UpdateProductHandler(
                repositoryMock.Object,
                cacheServiceMock.Object);

            var command = new UpdateProductCommand(
                product.Id,
                "Yeni Ürün",
                250m,
                20);

            // Act (aksiyon)
            await handler.Handle(command, CancellationToken.None);

            // Assert (doğrulama)
            Assert.Equal("Yeni Ürün", product.Name);
            Assert.Equal(250m, product.Price);
            Assert.Equal(20, product.StockQuantity);

            repositoryMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);

            cacheServiceMock.Verify(
                x => x.RemoveAsync($"product:{product.Id}"),
                Times.Once);
        }

        [Fact]
        public async Task Handle_Should_ThrowNotFoundException_When_ProductDoesNotExist()
        {
            // Arrange
            var repositoryMock = new Mock<IProductRepository>();
            var cacheServiceMock = new Mock<ICacheService>();

            repositoryMock
                .Setup(x => x.GetTrackedByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product.Domain.Entities.Product?)null);

            var handler = new UpdateProductHandler(
                repositoryMock.Object,
                cacheServiceMock.Object);

            var command = new UpdateProductCommand(
                Guid.NewGuid(),
                "Yeni Ürün",
                250m,
                20);

            // Act + Assert
            await Assert.ThrowsAsync<NotFoundException>(() =>
                handler.Handle(command, CancellationToken.None));

            repositoryMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Never);

            cacheServiceMock.Verify(
                x => x.RemoveAsync(It.IsAny<string>()),
                Times.Never);
        }
    }
}
