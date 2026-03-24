using BuildingBlocks.Domain;
using Product.Domain.ValueObjects;

namespace Product.Domain.Events
{
    // Yeni ürün oluşturulduğunda tetiklenen domain event
    public sealed class ProductCreatedDomainEvent : DomainEvent
    {
        // Oluşturulan ürünün kimliği
        public Guid ProductId { get; }

        public ProductCreatedDomainEvent(Guid productId)
        {
            ProductId = productId;
        }
    }
}
