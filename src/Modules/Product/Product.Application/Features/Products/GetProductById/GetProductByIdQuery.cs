using MediatR;
using Product.Application.Abstractions.Caching;

namespace Product.Application.Features.Products.GetProductById
{
    // Id'ye göre ürün getirme isteğini temsil eder
    public sealed record GetProductByIdQuery(Guid ProductId)
        : IRequest<ProductResponse>, ICacheableQuery
    {
        public string CacheKey => $"product:{ProductId}";

        public TimeSpan Expiration => TimeSpan.FromMinutes(5);
    }
}
