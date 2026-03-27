namespace BuildingBlocks.Domain
{
    // Oluşturulma ve güncellenme zamanını tutan temel entity sınıfı
    public abstract class AuditableEntity : BaseEntity
    {
        // Kaydın oluşturulma zamanı
        public DateTime CreatedAtUtc { get; protected set; }

        // Kaydın son güncellenme zamanı
        public DateTime? UpdatedAtUtc { get; protected set; }

        // Kaydın silinmiş olup olmadığını gösterir
        public bool IsDeleted { get; protected set; }

        // Kaydın silinme zamanı
        public DateTime? DeletedAtUtc { get; protected set; }

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

        // Silinmiş olarak işaretler
        public void MarkAsDeleted(DateTime deletedAtUtc)
        {
            IsDeleted = true;
            DeletedAtUtc = deletedAtUtc;
        }

        // Silinmiş işaretini kaldırır
        public void Restore()
        {
            IsDeleted = false;
            DeletedAtUtc = null;
        }
    }
}
