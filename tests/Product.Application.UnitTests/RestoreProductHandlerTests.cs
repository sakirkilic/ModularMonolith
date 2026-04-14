using BuildingBlocks.Domain.Exceptions;
using Moq;
using Product.Application.Abstractions.Caching;
using Product.Application.Abstractions.Data;
using Product.Application.Features.Products.RestoreProduct;

namespace Product.Application.UnitTests
{
    // RestoreProductHandler davranışlarını test eder
    public sealed class RestoreProductHandlerTests
    {
        [Fact]
        public async Task Handle_Should_RestoreProduct_When_ProductIsSoftDeleted()
        {
            // Arrange
            var productResult = Product.Domain.Entities.Product.Create("Silinmiş Ürün", 100m, 5);
            var product = productResult.Value;

            product.Delete(Guid.NewGuid());

            var repositoryMock = new Mock<IProductRepository>();
            var cacheServiceMock = new Mock<ICacheService>();

            repositoryMock
                .Setup(x => x.GetTrackedByIdIncludingDeletedAsync(product.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            var handler = new RestoreProductHandler(
                repositoryMock.Object,
                cacheServiceMock.Object);

            var command = new RestoreProductCommand(product.Id);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(product.IsDeleted);
            Assert.Null(product.DeletedAtUtc);
            Assert.Null(product.DeletedBy);

            repositoryMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);

            cacheServiceMock.Verify(
                x => x.RemoveAsync($"product:{product.Id}"),
                Times.Once);
        }

        [Fact]
        public async Task Handle_Should_ThrowBusinessRuleException_When_ProductIsAlreadyActive()
        {
            // Arrange
            var productResult = Product.Domain.Entities.Product.Create("Aktif Ürün", 100m, 5);
            var product = productResult.Value;

            var repositoryMock = new Mock<IProductRepository>();
            var cacheServiceMock = new Mock<ICacheService>();

            repositoryMock
                .Setup(x => x.GetTrackedByIdIncludingDeletedAsync(product.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            var handler = new RestoreProductHandler(
                repositoryMock.Object,
                cacheServiceMock.Object);

            var command = new RestoreProductCommand(product.Id);

            // Act + Assert
            await Assert.ThrowsAsync<BusinessRuleException>(() =>
                handler.Handle(command, CancellationToken.None));

            repositoryMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Never);

            cacheServiceMock.Verify(
                x => x.RemoveAsync(It.IsAny<string>()),
                Times.Never);
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

            var handler = new RestoreProductHandler(
                repositoryMock.Object,
                cacheServiceMock.Object);

            var command = new RestoreProductCommand(Guid.NewGuid());

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
