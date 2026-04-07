using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Product.Application.Abstractions.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Product.Infrastructure.Authentication
{
    // JWT token üretir
    public sealed class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _configuration;

        public JwtTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(Guid userId, string email, string role, IEnumerable<string> permissions)
        {
            var key = _configuration["Jwt:Key"];
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var expireMinutes = int.Parse(_configuration["Jwt:ExpireMinutes"]!);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            List<Claim> claims = new ()
            {
                new(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new(JwtRegisteredClaimNames.Email, email),
                new(ClaimTypes.Role, role)
            };

            foreach (var permission in permissions)
            {
                claims.Add(new Claim("permission", permission));
            }

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expireMinutes),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

/*
 target-typed new expressions syntax

 Eski yazım:
 var claims = new List<Claim>
 {
     new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
     new Claim(JwtRegisteredClaimNames.Email, email),
     new Claim(ClaimTypes.Role, role)
 };

Yeni Yazım: 
List<Claim> claims = new ()
{
    new(JwtRegisteredClaimNames.Sub, userId.ToString()),
    new(JwtRegisteredClaimNames.Email, email),
    new(ClaimTypes.Role, role)
};

JwtTokenService target_new_type = new(_configuration);
List<string> newList = new();


*/