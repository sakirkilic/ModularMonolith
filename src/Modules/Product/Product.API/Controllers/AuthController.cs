using Microsoft.AspNetCore.Mvc;
using Product.Application.Abstractions.Authentication;

namespace Product.API.Controllers
{
    // Authentication işlemleri
    [ApiController]
    [Route("api/auth")]
    public sealed class AuthController : ControllerBase
    {
        private readonly IJwtTokenService _jwtService;

        public AuthController(IJwtTokenService jwtService)
        {
            _jwtService = jwtService;
        }

        // Demo login endpoint
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var userId = Guid.NewGuid();
            var email = request.Email;
            var role = request.Role;

            var token = _jwtService.GenerateToken(userId, email, role);

            return Ok(new
            {
                token,
                email,
                role
            });
        }
    }
}
