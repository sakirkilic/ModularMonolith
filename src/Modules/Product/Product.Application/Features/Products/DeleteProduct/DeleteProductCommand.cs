using MediatR;

namespace Product.Application.Features.Products.DeleteProduct
{
    // Ürün silme isteğini temsil eder
    public sealed record DeleteProductCommand(Guid ProductId) : IRequest;
}
