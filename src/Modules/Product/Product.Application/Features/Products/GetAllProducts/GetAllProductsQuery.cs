using MediatR;

namespace Product.Application.Features.Products.GetAllProducts
{
    // Tüm ürünleri getirme isteğini temsil eder
    public sealed record GetAllProductsQuery() : IRequest<List<ProductListItemResponse>>;
}
