namespace Product.Application.Abstractions.Authentication
{
    // JWT token üretme sözleşmesi
    public interface IJwtTokenService
    {
        string GenerateToken(Guid userId, string email, string role, IEnumerable<string> permissions);
    }
}
