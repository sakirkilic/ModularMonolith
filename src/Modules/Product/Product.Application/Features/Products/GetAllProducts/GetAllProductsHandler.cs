using BuildingBlocks.Domain.Primitives;
using MediatR;
using Product.Application.Abstractions.Data;

namespace Product.Application.Features.Products.GetAllProducts
{
    // Tüm ürünleri sayfalı olarak getirme işlemini yöneten handler
    public sealed class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, PagedResult<ProductListItemResponse>>
    {
        private readonly IProductRepository _productRepository;

        public GetAllProductsHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // Ürünleri sayfalı şekilde veritabanından getirir
        public async Task<PagedResult<ProductListItemResponse>> Handle(
            GetAllProductsQuery request,
            CancellationToken cancellationToken)
        {
            var (items, totalCount) = await _productRepository.GetPagedAsync(
                request.Page,
                request.PageSize,
                request.Search,
                request.MinPrice,
                request.MaxPrice,
                request.SortBy,
                request.SortDirection,
                cancellationToken);

            var responseItems = items
                .Select(product => new ProductListItemResponse(
                    product.Id,
                    product.Name,
                    product.Price,
                    product.StockQuantity))
                .ToList();

            return new PagedResult<ProductListItemResponse>(
                responseItems,
                totalCount,
                request.Page,
                request.PageSize);
        }
    }
}
