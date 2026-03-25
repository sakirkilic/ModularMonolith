using MediatR;
using Product.Application.Abstractions.Data;

namespace Product.Application.Features.Products.CreateProduct
{
    // Ürün oluşturma işlemini yöneten handler
    public sealed class CreateProductHandler
        : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly IProductRepository _productRepository;

        public CreateProductHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // Ürün oluşturma işlemini gerçekleştirir
        public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var result = Product.Domain.Entities.Product.Create(
                request.Name,
                request.Price,
                request.StockQuantity);

            if (result.IsFailure)
            {
                throw new Exception(result.Error.Message);
            }
            var product = result.Value;

            await _productRepository.AddAsync(product, cancellationToken);
            await _productRepository.SaveChangesAsync(cancellationToken);

            return product.Id;
        }
    }
}
