namespace BuildingBlocks.Domain
{
    // Oluşturulma, güncellenme ve silinme bilgisini tutan temel entity sınıfı
    public abstract class AuditableEntity : BaseEntity
    {
        // Kaydın oluşturulma zamanı
        public DateTime CreatedAtUtc { get; protected set; }

        // Kaydı oluşturan kullanıcı
        public Guid? CreatedBy { get; protected set; }

        // Kaydın son güncellenme zamanı
        public DateTime? UpdatedAtUtc { get; protected set; }

        // Kaydı son güncelleyen kullanıcı
        public Guid? UpdatedBy { get; protected set; }

        // Kaydın silinmiş olup olmadığını gösterir
        public bool IsDeleted { get; protected set; }

        // Kaydın silinme zamanı
        public DateTime? DeletedAtUtc { get; protected set; }

        // Kaydı silen kullanıcı
        public Guid? DeletedBy { get; protected set; }

        // Oluşturulma zamanını ayarlar
        public void SetCreatedAt(DateTime createdAtUtc)
        {
            CreatedAtUtc = createdAtUtc;
        }

        // Kaydı oluşturan kullanıcıyı ayarlar
        public void SetCreatedBy(Guid? createdBy)
        {
            CreatedBy = createdBy;
        }

        // Güncellenme zamanını ayarlar
        public void SetUpdatedAt(DateTime updatedAtUtc)
        {
            UpdatedAtUtc = updatedAtUtc;
        }

        // Kaydı güncelleyen kullanıcıyı ayarlar
        public void SetUpdatedBy(Guid? updatedBy)
        {
            UpdatedBy = updatedBy;
        }

        // Silinmiş olarak işaretler
        public void MarkAsDeleted(DateTime deletedAtUtc, Guid? deletedBy)
        {
            IsDeleted = true;
            DeletedAtUtc = deletedAtUtc;
            DeletedBy = deletedBy;
        }

        // Silinmiş işaretini kaldırır
        public void Restore()
        {
            IsDeleted = false;
            DeletedAtUtc = null;
            DeletedBy = null;
        }
    }

}
