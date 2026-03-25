using MediatR;
using Product.Application.Abstractions.Data;

namespace Product.Application.Features.Products.GetAllProducts
{
    // Tüm ürünleri getirme işlemini yöneten handler
    public sealed class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, List<ProductListItemResponse>>
    {
        private readonly IProductRepository _productRepository;

        public GetAllProductsHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // Tüm ürünleri veritabanından getirir
        public async Task<List<ProductListItemResponse>> Handle(
            GetAllProductsQuery request,
            CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetAllAsync(cancellationToken);

            return products
                .Select(product => new ProductListItemResponse(
                    product.Id,
                    product.Name,
                    product.Price,
                    product.StockQuantity))
                .ToList();
        }
    }
}
