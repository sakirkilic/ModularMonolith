using BuildingBlocks.Domain.Primitives;
using MediatR;

namespace Product.Application.Features.Products.GetAllProducts
{
    // Sayfalı ve filtreli ürün listeleme isteğini temsil eder
    public sealed record GetAllProductsQuery(
        int Page,
        int PageSize,
        string? Search,
        decimal? MinPrice,
        decimal? MaxPrice
    ) : IRequest<PagedResult<ProductListItemResponse>>;
}
