using BuildingBlocks.Domain.Exceptions;
using MediatR;
using Product.Application.Abstractions.Caching;
using Product.Application.Abstractions.Data;

namespace Product.Application.Features.Products.HardDeleteProduct
{
    // Ürünü fiziksel olarak silme işlemini yöneten handler
    public sealed class HardDeleteProductHandler : IRequestHandler<HardDeleteProductCommand>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICacheService _cacheService;

        public HardDeleteProductHandler(IProductRepository productRepository, ICacheService cacheService)
        {
            _productRepository = productRepository;
            _cacheService = cacheService;
        }

        // Ürünü fiziksel olarak veritabanından siler
        public async Task Handle(HardDeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetTrackedByIdIncludingDeletedAsync(
                request.ProductId,
                cancellationToken);

            if (product is null)
            {
                throw new NotFoundException("Kalıcı olarak silinecek ürün bulunamadı.");
            }

            _productRepository.HardRemove(product);

            await _productRepository.SaveChangesAsync(cancellationToken);

            await _cacheService.RemoveAsync($"product:{request.ProductId}");
        }
    }
}
