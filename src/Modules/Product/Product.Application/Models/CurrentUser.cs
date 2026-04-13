namespace Product.Application.Models
{
    // Mevcut kullanıcı bilgisini temsil eder
    public sealed record CurrentUser(
        Guid? UserId,
        string? Email,
        string? Role,
        bool IsAuthenticated
    );
}
