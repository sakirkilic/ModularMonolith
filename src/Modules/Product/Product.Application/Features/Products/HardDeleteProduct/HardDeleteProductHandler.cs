using BuildingBlocks.Domain.Exceptions;
using MediatR;
using Product.Application.Abstractions.Data;

namespace Product.Application.Features.Products.HardDeleteProduct
{
    // Ürünü fiziksel olarak silme işlemini yöneten handler
    public sealed class HardDeleteProductHandler : IRequestHandler<HardDeleteProductCommand>
    {
        private readonly IProductRepository _productRepository;

        public HardDeleteProductHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
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
        }
    }
}
