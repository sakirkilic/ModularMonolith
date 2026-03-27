using MediatR;

namespace Product.Application.Features.Products.UpdateProduct
{
    // Ürün güncelleme isteğini temsil eder
    public sealed record UpdateProductCommand(
        Guid ProductId,
        string Name,
        decimal Price,
        int StockQuantity
    ) : IRequest; 
}
