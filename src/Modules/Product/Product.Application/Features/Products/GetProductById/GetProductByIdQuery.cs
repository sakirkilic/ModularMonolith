using MediatR;

namespace Product.Application.Features.Products.GetProductById
{
    // Id'ye göre ürün getirme isteğini temsil eder
    public sealed record GetProductByIdQuery(Guid ProductId) : IRequest<ProductResponse>;
}
