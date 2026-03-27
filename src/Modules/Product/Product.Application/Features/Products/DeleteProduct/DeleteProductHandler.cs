using BuildingBlocks.Domain.Exceptions;
using MediatR;
using Product.Application.Abstractions.Data;

namespace Product.Application.Features.Products.DeleteProduct
{
    // Ürün silme işlemini yöneten handler
    public sealed class DeleteProductHandler : IRequestHandler<DeleteProductCommand>
    {
        private readonly IProductRepository _productRepository;

        public DeleteProductHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // Ürünü siler
        public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetTrackedByIdAsync(request.ProductId, cancellationToken);

            if (product is null)
            {
                throw new NotFoundException("Silinecek ürün bulunamadı.");
            }

            _productRepository.Remove(product);

            await _productRepository.SaveChangesAsync(cancellationToken);
        }
    }
}
