using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Domain
{
    // Domain içinde gerçekleşen olayların temel sınıfı
    public abstract class DomainEvent
    {
        // Event'in oluştuğu zaman (UTC)
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
    }
}
