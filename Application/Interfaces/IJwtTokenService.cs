using ims.Infrastructure.Identity;
using System.Security.Claims;

namespace ims.Application.Interfaces
{
    public class IJwtTokenService
    {
        Task<string> GenerateAccessTokenAsync(
        ApplicationUser user,
        IEnumerable<string> roles,
        IEnumerable<Claim>? extraClaims = null,
        string? jti = null);

        string GenerateRefreshToken();
        string HashToken(string token);
    }
}
