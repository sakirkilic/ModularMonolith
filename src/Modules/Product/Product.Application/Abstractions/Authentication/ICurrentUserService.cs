using Product.Application.Models;

namespace Product.Application.Abstractions.Authentication
{
    // Mevcut kullanıcı bilgisini sağlayan sözleşme
    public interface ICurrentUserService
    {
        CurrentUser GetCurrentUser();
    }
}
