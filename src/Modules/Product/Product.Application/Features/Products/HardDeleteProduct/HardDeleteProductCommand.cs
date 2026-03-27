using MediatR;

namespace Product.Application.Features.Products.HardDeleteProduct
{
    // Ürünü fiziksel olarak silme isteğini temsil eder
    public sealed record HardDeleteProductCommand(Guid ProductId) : IRequest;
}
