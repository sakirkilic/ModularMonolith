using BuildingBlocks.Domain.Exceptions;
using MediatR;
using Product.Application.Abstractions.Data;

namespace Product.Application.Features.Products.RestoreProduct
{
    // Silinmiş ürünü geri yükleme işlemini yöneten handler
    public sealed class RestoreProductHandler : IRequestHandler<RestoreProductCommand>
    {
        private readonly IProductRepository _productRepository;

        public RestoreProductHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // Soft delete edilmiş ürünü tekrar aktif hale getirir
        public async Task Handle(RestoreProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetTrackedByIdIncludingDeletedAsync(
                request.ProductId,
                cancellationToken);

            if (product is null)
            {
                throw new NotFoundException("Geri yüklenecek ürün bulunamadı.");
            }

            if (!product.IsDeleted)
            {
                throw new BusinessRuleException("Ürün zaten aktif durumda.");
            }

            product.Restore();

            await _productRepository.SaveChangesAsync(cancellationToken);
        }
    }
}
