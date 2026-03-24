using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Domain
{
    // Tüm entity'ler için temel sınıf (Id + domain event yönetimi)
    public abstract class BaseEntity
    {
        // Entity üzerinde oluşan domain event'leri tutar
        private readonly List<DomainEvent> _domainEvents = new();

        // Entity'nin benzersiz kimliği
        public Guid Id { get; protected set; }

        // Domain event'leri dışarıya sadece okunabilir olarak verir
        public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        // Yeni bir domain event ekler
        protected void AddDomainEvent(DomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        // Event listesi işlendiğinde temizlenir
        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}
