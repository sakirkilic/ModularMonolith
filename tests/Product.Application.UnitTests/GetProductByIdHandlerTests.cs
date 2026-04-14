using BuildingBlocks.Domain.Exceptions;
using Moq;
using Product.Application.Abstractions.Data;
using Product.Application.Features.Products.GetProductById;

namespace Product.Application.UnitTests
{
    // GetProductByIdHandler davranışlarını test eder
    public sealed class GetProductByIdHandlerTests
    {
        [Fact]
        public async Task Handle_Should_ReturnProductResponse_When_ProductExists()
        {
            // Arrange
            var productResult = Product.Domain.Entities.Product.Create("Laptop", 1000m, 10);
            var product = productResult.Value;

            var repositoryMock = new Mock<IProductRepository>();

            repositoryMock
                .Setup(x => x.GetByIdAsync(product.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            var handler = new GetProductByIdHandler(repositoryMock.Object);
            var query = new GetProductByIdQuery(product.Id);

            // Act
            var response = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(product.Id, response.Id);
            Assert.Equal(product.Name, response.Name);
            Assert.Equal(product.Price, response.Price);
            Assert.Equal(product.StockQuantity, response.StockQuantity);
        }

        [Fact]
        public async Task Handle_Should_ThrowNotFoundException_When_ProductDoesNotExist()
        {
            // Arrange
            var repositoryMock = new Mock<IProductRepository>();

            repositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product.Domain.Entities.Product?)null);

            var handler = new GetProductByIdHandler(repositoryMock.Object);
            var query = new GetProductByIdQuery(Guid.NewGuid());

            // Act + Assert
            await Assert.ThrowsAsync<NotFoundException>(() =>
                handler.Handle(query, CancellationToken.None));
        }
    }
}
