using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Product.Application.Abstractions.Authentication;
using Product.Application.Security;

namespace Product.API.Controllers
{
    // Authentication işlemleri
    [ApiController]
    [Route("api/auth")]
    public sealed class AuthController : ControllerBase
    {
        private readonly IJwtTokenService _jwtService;
        private readonly ICurrentUserService _currentUserService;

        public AuthController(IJwtTokenService jwtService, ICurrentUserService currentUserService)
        {
            _jwtService = jwtService;
            _currentUserService = currentUserService;
        }

        // Demo login endpoint
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var userId = Guid.NewGuid();
            var email = request.Email;
            var role = request.Role;

            var permissions = GetPermissionsByRole(role);

            var token = _jwtService.GenerateToken(userId, email, role, permissions);

            return Ok(new
            {
                token,
                email,
                role,
                permissions
            });
        }

        // Mevcut kullanıcı bilgisini döner
        [Authorize]
        [HttpGet("me")]
        public IActionResult Me()
        {
            var currentUser = _currentUserService.GetCurrentUser();

            return Ok(currentUser);
        }

        // Role'e göre permission (yetki) listesini üretir
        private static List<string> GetPermissionsByRole(string role)
        {
            var normalizedRole = role.Trim().ToLowerInvariant();

            return normalizedRole switch
            {
                "admin" =>
                [
                    ProductPermissions.Manage,
            ProductPermissions.HardDelete
                ],

                "productmanager" =>
                [
                    ProductPermissions.Manage
                ],

                "user" => [],

                _ => []
            };
        }
    }

