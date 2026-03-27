namespace BuildingBlocks.Domain
{
    // Oluşturulma ve güncellenme zamanını tutan temel entity sınıfı
    public abstract class AuditableEntity : BaseEntity
    {
        // Kaydın oluşturulma zamanı
        public DateTime CreatedAtUtc { get; protected set; }

        // Kaydın son güncellenme zamanı
        public DateTime? UpdatedAtUtc { get; protected set; }

        // Oluşturulma zamanını ayarlar
        public void SetCreatedAt(DateTime createdAtUtc)
        {
            CreatedAtUtc = createdAtUtc;
        }

        // Güncellenme zamanını ayarlar
        public void SetUpdatedAt(DateTime updatedAtUtc)
        {
            UpdatedAtUtc = updatedAtUtc;
        }
    }
}
