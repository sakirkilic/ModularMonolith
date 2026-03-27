using MediatR;

namespace Product.Application.Features.Products.RestoreProduct
{
    // Silinmiş ürünü geri getirme isteğini temsil eder
    public sealed record RestoreProductCommand(Guid ProductId) : IRequest;
}
