using BuildingBlocks.Domain.Exceptions;
using MediatR;
using Product.Application.Abstractions.Authentication;
using Product.Application.Abstractions.Caching;
using Product.Application.Abstractions.Data;

namespace Product.Application.Features.Products.DeleteProduct
{
    // Ürün silme işlemini yöneten handler
    public sealed class DeleteProductHandler : IRequestHandler<DeleteProductCommand>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICacheService _cacheService;
        private readonly ICurrentUserService _currentUserService;

        public DeleteProductHandler(IProductRepository productRepository, ICacheService cacheService, ICurrentUserService currentUserService)
        {
            _productRepository = productRepository;
            _cacheService = cacheService;
            _currentUserService = currentUserService;
        }

        // Ürünü silinmiş olarak işaretler
        public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetTrackedByIdAsync(request.ProductId, cancellationToken);

            if (product is null)
            {
                throw new NotFoundException("Silinecek ürün bulunamadı.");
            }

            var currentUser = _currentUserService.GetCurrentUser();

            product.Delete(currentUser.UserId);

            await _productRepository.SaveChangesAsync(cancellationToken);

            await _cacheService.RemoveAsync($"product:{request.ProductId}");
        }
    }
}
