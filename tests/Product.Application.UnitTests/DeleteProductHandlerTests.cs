using BuildingBlocks.Domain.Exceptions;
using Moq;
using Product.Application.Abstractions.Authentication;
using Product.Application.Abstractions.Caching;
using Product.Application.Abstractions.Data;
using Product.Application.Features.Products.DeleteProduct;
using Product.Application.Models;

namespace Product.Application.UnitTests
{
    // DeleteProductHandler davranışlarını test eder
    public sealed class DeleteProductHandlerTests
    {
        [Fact]
        public async Task Handle_Should_SoftDeleteProduct_When_ProductExists()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var productResult = Product.Domain.Entities.Product.Create("Silinecek Ürün", 100m, 5);
            var product = productResult.Value;

            var repositoryMock = new Mock<IProductRepository>();
            var cacheServiceMock = new Mock<ICacheService>();
            var currentUserServiceMock = new Mock<ICurrentUserService>();

            repositoryMock
                .Setup(x => x.GetTrackedByIdAsync(product.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            currentUserServiceMock
                .Setup(x => x.GetCurrentUser())
                .Returns(new CurrentUser(
                    userId,
                    "admin@test.com",
                    "Admin",
                    true));

            var handler = new DeleteProductHandler(
                repositoryMock.Object,
                cacheServiceMock.Object,
                currentUserServiceMock.Object);

            var command = new DeleteProductCommand(product.Id);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(product.IsDeleted);
            Assert.NotNull(product.DeletedAtUtc);
            Assert.Equal(userId, product.DeletedBy);

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
            var currentUserServiceMock = new Mock<ICurrentUserService>();

            repositoryMock
                .Setup(x => x.GetTrackedByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product.Domain.Entities.Product?)null);

            var handler = new DeleteProductHandler(
                repositoryMock.Object,
                cacheServiceMock.Object,
                currentUserServiceMock.Object);

            var command = new DeleteProductCommand(Guid.NewGuid());

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
