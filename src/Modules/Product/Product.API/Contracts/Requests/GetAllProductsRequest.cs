using BuildingBlocks.Domain.Primitives;
using MediatR;
using Product.Application.Features.Products.GetAllProducts;

namespace Product.API.Contracts.Requests
{
    // Ürün listeleme API isteğini temsil eder
    public sealed record GetAllProductsRequest(
        int Page = 1,
        int PageSize = 10
    );
}
