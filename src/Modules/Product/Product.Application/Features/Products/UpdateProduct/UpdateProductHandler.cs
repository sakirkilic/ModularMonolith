using BuildingBlocks.Domain.Exceptions;
using MediatR;
using Product.Application.Abstractions.Caching;
using Product.Application.Abstractions.Data;

namespace Product.Application.Features.Products.UpdateProduct
{
    // Ürün güncelleme işlemini yöneten handler
    public sealed class UpdateProductHandler : IRequestHandler<UpdateProductCommand>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICacheService _cacheService;

        public UpdateProductHandler(IProductRepository productRepository, ICacheService cacheService)
        {
            _productRepository = productRepository;
            _cacheService = cacheService;
        }

        // Ürünü günceller
        public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetTrackedByIdAsync(request.ProductId, cancellationToken);

            if (product is null)
            {
                throw new NotFoundException("Güncellenecek ürün bulunamadı.");
            }

            var nameResult = product.ChangeName(request.Name);
            if (nameResult.IsFailure)
            {
                throw new BusinessRuleException(nameResult.Error.Message);
            }

            var priceResult = product.ChangePrice(request.Price);
            if (priceResult.IsFailure)
            {
                throw new BusinessRuleException(priceResult.Error.Message);
            }

            var stockResult = product.UpdateStock(request.StockQuantity);
            if (stockResult.IsFailure)
            {
                throw new BusinessRuleException(stockResult.Error.Message);
            }

            await _productRepository.SaveChangesAsync(cancellationToken);

            await _cacheService.RemoveAsync($"product:{request.ProductId}");
        }
    }
}
