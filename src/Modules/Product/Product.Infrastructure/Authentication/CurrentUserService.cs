using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Product.Application.Abstractions.Authentication;
using Product.Application.Models;

namespace Product.Infrastructure.Authentication
{
    // HttpContext üzerinden mevcut kullanıcı bilgisini üretir
    public sealed class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // Mevcut kullanıcıyı döner
        public CurrentUser GetCurrentUser()
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user is null || user.Identity is null || !user.Identity.IsAuthenticated)
            {
                return new CurrentUser(
                    UserId: null,
                    Email: null,
                    Role: null,
                    IsAuthenticated: false);
            }

            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? user.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            Guid? userId = null;

            if (Guid.TryParse(userIdClaim, out var parsedUserId))
            {
                userId = parsedUserId;
            }

            var email = user.FindFirst(ClaimTypes.Email)?.Value
                ?? user.FindFirst(JwtRegisteredClaimNames.Email)?.Value;

            var role = user.FindFirst(ClaimTypes.Role)?.Value;

            return new CurrentUser(
                UserId: userId,
                Email: email,
                Role: role,
                IsAuthenticated: true);
        }
    }
}
