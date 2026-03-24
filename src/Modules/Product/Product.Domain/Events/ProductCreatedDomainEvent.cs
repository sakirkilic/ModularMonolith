using BuildingBlocks.Domain;
using Product.Domain.ValueObjects;

namespace Product.Domain.Events
{
    // Product oluşturulduğunda tetiklenir
    public sealed class ProductCreatedDomainEvent : DomainEvent
    {
        public ProductId ProductId { get; }

        public ProductCreatedDomainEvent(ProductId productId)
        {
            ProductId = productId;
        }
    }
}
