using BuildingBlocks.Domain.Exceptions;
using MediatR;
using Product.Application.Abstractions.Caching;
using Product.Application.Abstractions.Data;

namespace Product.Application.Features.Products.DeleteProduct
{
    // Ürün silme işlemini yöneten handler
    public sealed class DeleteProductHandler : IRequestHandler<DeleteProductCommand>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICacheService _cacheService;

        public DeleteProductHandler(IProductRepository productRepository, ICacheService cacheService)
        {
            _productRepository = productRepository;
            _cacheService = cacheService;
        }

        // Ürünü silinmiş olarak işaretler
        public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetTrackedByIdAsync(request.ProductId, cancellationToken);

            if (product is null)
            {
                throw new NotFoundException("Silinecek ürün bulunamadı.");
            }

            product.Delete();

            await _productRepository.SaveChangesAsync(cancellationToken);

            await _cacheService.RemoveAsync($"product:{request.ProductId}");
        }
    }
}
