using BuildingBlocks.Domain.Exceptions;
using Moq;
using Product.Application.Abstractions.Caching;
using Product.Application.Abstractions.Data;
using Product.Application.Features.Products.HardDeleteProduct;

namespace Product.Application.UnitTests
{
    // HardDeleteProductHandler davranışlarını test eder
    public sealed class HardDeleteProductHandlerTests
    {
        [Fact]
        public async Task Handle_Should_HardDeleteProduct_When_ProductExists()
        {
            // Arrange (hazırlık)
            var productResult = Product.Domain.Entities.Product.Create("Kalıcı Silinecek Ürün", 100m, 5);
            var product = productResult.Value;

            var repositoryMock = new Mock<IProductRepository>();
            var cacheServiceMock = new Mock<ICacheService>();

            repositoryMock
                .Setup(x => x.GetTrackedByIdIncludingDeletedAsync(product.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            var handler = new HardDeleteProductHandler(
                repositoryMock.Object,
                cacheServiceMock.Object);

            var command = new HardDeleteProductCommand(product.Id);

            // Act (aksiyon)
            await handler.Handle(command, CancellationToken.None);

            // Assert (doğrulama)
            repositoryMock.Verify(
                x => x.HardRemove(product),
                Times.Once);

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
                .Setup(x => x.GetTrackedByIdIncludingDeletedAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product.Domain.Entities.Product?)null);

            var handler = new HardDeleteProductHandler(
                repositoryMock.Object,
                cacheServiceMock.Object);

            var command = new HardDeleteProductCommand(Guid.NewGuid());

            // Act + Assert
            await Assert.ThrowsAsync<NotFoundException>(() =>
                handler.Handle(command, CancellationToken.None));

            repositoryMock.Verify(
                x => x.HardRemove(It.IsAny<Product.Domain.Entities.Product>()),
                Times.Never);

            repositoryMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Never);

            cacheServiceMock.Verify(
                x => x.RemoveAsync(It.IsAny<string>()),
                Times.Never);
        }
    }
}
