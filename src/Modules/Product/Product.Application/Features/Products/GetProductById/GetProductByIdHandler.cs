using BuildingBlocks.Domain.Exceptions;
using MediatR;
using Product.Application.Abstractions.Caching;
using Product.Application.Abstractions.Data;

namespace Product.Application.Features.Products.GetProductById
{
    // Id'ye göre ürün getirme işlemini yöneten handler
    public sealed class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, ProductResponse>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICacheService _cache;

        public GetProductByIdHandler(IProductRepository productRepository, ICacheService cache)
        {
            _productRepository = productRepository;
            _cache = cache;
        }

        // Ürünü veritabanından getirir
        public async Task<ProductResponse> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            //var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);

            //if (product is null)
            //{
            //    throw new NotFoundException("Ürün bulunamadı.");
            //}

            //return new ProductResponse(
            //    product.Id,
            //    product.Name,
            //    product.Price,
            //    product.StockQuantity);


            var cacheKey = $"product:{request.ProductId}";

            // 1. cache kontrolü
            var cached = await _cache.GetAsync<ProductResponse>(cacheKey);

            if (cached is not null)
            {
                return cached;
            }

            // 2. DB'den getir
            var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);

            if (product is null)
            {
                throw new NotFoundException("Ürün bulunamadı");
            }

            var response = new ProductResponse(
                product.Id,
                product.Name,
                product.Price,
                product.StockQuantity);

            // 3. cache'e yaz
            await _cache.SetAsync(cacheKey, response, TimeSpan.FromMinutes(5));

            return response;
        }
    }
}
