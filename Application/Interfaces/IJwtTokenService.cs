using ims.Infrastructure.Identity;
using System.Security.Claims;

namespace ims.Application.Interfaces;

public interface IJwtTokenService
{
    Task<string> GenerateAccessTokenAsync(
        ApplicationUser user,
        IEnumerable<string> roles,
        IEnumerable<Claim>? extraClaims = null,
        string? jti = null);

    string GenerateRefreshToken();
    string HashToken(string token);
}

