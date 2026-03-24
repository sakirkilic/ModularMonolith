using MediatR;

namespace Product.Application.Features.Products.CreateProduct
{
    // Ürün oluşturma işlemini yöneten handler
    public sealed class CreateProductHandler
        : IRequestHandler<CreateProductCommand, Guid>
    {
        public Task<Guid> Handle(
            CreateProductCommand request,
            CancellationToken cancellationToken)
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

            // Şimdilik sadece Id dönüyoruz (db yok)
            return Task.FromResult(product.Id);
        }
    }
}
