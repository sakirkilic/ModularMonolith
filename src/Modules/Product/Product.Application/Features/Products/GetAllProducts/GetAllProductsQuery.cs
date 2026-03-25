using BuildingBlocks.Domain.Primitives;
using MediatR;

namespace Product.Application.Features.Products.GetAllProducts
{
    // Sayfalı ürün listeleme isteğini temsil eder
    public sealed record GetAllProductsQuery(
        int Page,
        int PageSize
    ) : IRequest<PagedResult<ProductListItemResponse>>;
}
