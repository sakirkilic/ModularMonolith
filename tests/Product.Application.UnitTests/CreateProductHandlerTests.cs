using Moq;
using Product.Application.Abstractions.Data;
using Product.Application.Features.Products.CreateProduct;

namespace Product.Application.UnitTests
{
    // CreateProductHandler davranışlarını test eder
    public sealed class CreateProductHandlerTests
    {
        [Fact]
        public async Task Handle_Should_CreateProduct_When_CommandIsValid()
        {
            // Arrange
            var repositoryMock = new Mock<IProductRepository>();

            var handler = new CreateProductHandler(repositoryMock.Object);

            var command = new CreateProductCommand(
                "Yeni Ürün",
                500m,
                8);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotEqual(Guid.Empty, result);

            repositoryMock.Verify(
                x => x.AddAsync(It.Is<Product.Domain.Entities.Product>(p =>
                    p.Name == "Yeni Ürün" &&
                    p.Price == 500m &&
                    p.StockQuantity == 8),
                It.IsAny<CancellationToken>()),
                Times.Once);

            repositoryMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
