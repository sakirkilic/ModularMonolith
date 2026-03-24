using MediatR;

namespace Product.Application.Features.Products.CreateProduct
{
    // Ürün oluşturma isteğini temsil eder
    public sealed record CreateProductCommand(
        string Name,
        decimal Price,
        int StockQuantity
    ) : IRequest<Guid>;
}
