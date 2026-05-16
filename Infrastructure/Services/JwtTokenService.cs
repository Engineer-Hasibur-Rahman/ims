using ims.Application.Interfaces;
using ims.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ims.Infrastructure.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public JwtTokenService(
            IConfiguration configuration,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            _configuration = configuration;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<string> GenerateAccessTokenAsync(
            ApplicationUser user,
            IEnumerable<string> roles,
            IEnumerable<Claim>? extraClaims = null,
            string? jti = null)
        {
            var jwtSection = _configuration.GetSection("Jwt");
            var key = jwtSection["Key"]?.Trim() ?? throw new InvalidOperationException("JWT key is missing.");
            var issuer = jwtSection["Issuer"]?.Trim() ?? throw new InvalidOperationException("JWT issuer is missing.");
            var audience = jwtSection["Audience"]?.Trim() ?? throw new InvalidOperationException("JWT audience is missing.");
            var minutes = int.Parse(jwtSection["AccessTokenMinutes"] ?? "60");

            var claims = new List<Claim>
        {
           new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.UserName ?? user.Email ?? string.Empty),
            new(ClaimTypes.Email, user.Email ?? string.Empty),
            new(ClaimTypes.GivenName, user.FirstName),
            new(ClaimTypes.Surname, user.LastName),          
            new(JwtRegisteredClaimNames.Jti, jti ?? Guid.NewGuid().ToString())
        };

            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var roleClaims = new List<Claim>();
            foreach (var roleName in roles)
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role is null) continue;

                var claimsForRole = await _roleManager.GetClaimsAsync(role);
                roleClaims.AddRange(claimsForRole);
            }

            claims.AddRange(roleClaims);
            claims.AddRange(extraClaims ?? Enumerable.Empty<Claim>());

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims.DistinctBy(x => new { x.Type, x.Value }),
                expires: DateTime.UtcNow.AddMinutes(minutes),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var bytes = RandomNumberGenerator.GetBytes(64);
            return Convert.ToBase64String(bytes);
        }

        public string HashToken(string token)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(token));
            return Convert.ToBase64String(bytes);
        }
    }

}
