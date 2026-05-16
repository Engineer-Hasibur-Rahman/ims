using Azure.Core;
using ims.Application.DTOs;
using ims.Application.DTOs.Auth;
using ims.Application.Interfaces;
using ims.Domain.Entities;
using ims.Infrastructure.Data;
using ims.Infrastructure.Identity;
using ims.Shared.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace ims.Application.Services;

public class AuthService : IAuthService
{
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly AppDbContext _dbContext;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IEmailService _emailService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager,
            AppDbContext dbContext,
            IJwtTokenService jwtTokenService,
            IEmailService emailService,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _dbContext = dbContext;
            _jwtTokenService = jwtTokenService;
            _emailService = emailService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResponseDto<LoginResponseDto>> RegisterAsync(RegisterRequestDto request)
        {
            var existing = await _userManager.FindByEmailAsync(request.Email);
            if (existing is not null)
                return ApiResponseDto<LoginResponseDto>.Fail("Email already exists.");

            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                IsActive = true,
                EmailConfirmed = false
            };

            var createResult = await _userManager.CreateAsync(user, request.Password);
            if (!createResult.Succeeded)
                return ApiResponseDto<LoginResponseDto>.Fail(
                    "Registration failed.",
                    createResult.Errors.Select(x => x.Description).ToArray());

            if (await _roleManager.RoleExistsAsync("Staff"))
            {
                await _userManager.AddToRoleAsync(user, "Staff");
            }

            var emailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            await _emailService.SendAsync(
                user.Email!,
                "Confirm your account",
                $"Use this token to confirm your account: {emailToken}");

            return ApiResponseDto<LoginResponseDto>.Ok(new LoginResponseDto
            {
                UserId = user.Id,
                Email = user.Email!,
                FullName = user.FullName
            }, "Registration successful. Please confirm your email.");
        }

        public async Task<ApiResponseDto<LoginResponseDto>> LoginAsync(LoginRequestDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
                return ApiResponseDto<LoginResponseDto>.Fail("Invalid credentials.");

            if (!user.IsActive || user.IsDeleted)
                return ApiResponseDto<LoginResponseDto>.Fail("Your account is disabled.");

            var signInResult = await _signInManager.PasswordSignInAsync(
                user.UserName!,
                request.Password,
                request.RememberMe,
                lockoutOnFailure: true);

            if (signInResult.IsLockedOut)
                return ApiResponseDto<LoginResponseDto>.Fail("Account locked due to repeated failed attempts.");

            if (!signInResult.Succeeded)
                return ApiResponseDto<LoginResponseDto>.Fail("Invalid credentials.");

            if (!user.EmailConfirmed)
                return ApiResponseDto<LoginResponseDto>.Fail("Email not confirmed.");

            var roles = await _userManager.GetRolesAsync(user);
            var jti = Guid.NewGuid().ToString();

            var permissions = await GetPermissionsForUserAsync(user);

            var accessToken = await _jwtTokenService.GenerateAccessTokenAsync(user, roles, jti: jti);
            var refreshToken = _jwtTokenService.GenerateRefreshToken();
            var refreshTokenHash = _jwtTokenService.HashToken(refreshToken);

            var refreshTokenEntity = new RefreshToken
            {
                UserId = user.Id,
                TokenHash = refreshTokenHash,
                JwtId = jti,
                ExpiresAt = DateTime.UtcNow.AddDays(int.Parse("7")),
                CreatedByIp = GetIpAddress(),
                CreatedBy = user.Email ?? user.UserName ?? string.Empty
            };

            _dbContext.RefreshTokens.Add(refreshTokenEntity);
            await _dbContext.SaveChangesAsync();

            return ApiResponseDto<LoginResponseDto>.Ok(new LoginResponseDto
            {
                UserId = user.Id,
                Email = user.Email!,
                FullName = user.FullName,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccessTokenExpiresAt = DateTime.UtcNow.AddMinutes(60),
                Roles = roles.ToList(),
                Permissions = permissions.ToList()
            }, "Login successful.");
        }

        public async Task<ApiResponseDto<LoginResponseDto>> RefreshTokenAsync(RefreshTokenDto request)
        {
            var tokenHash = _jwtTokenService.HashToken(request.RefreshToken);
            var storedToken = await _dbContext.RefreshTokens.FirstOrDefaultAsync(x => x.TokenHash == tokenHash);

            if (storedToken is null || !storedToken.IsActive)
                return ApiResponseDto<LoginResponseDto>.Fail("Invalid refresh token.");

            var user = await _userManager.FindByIdAsync(storedToken.UserId.ToString());
            if (user is null || !user.IsActive || user.IsDeleted)
                return ApiResponseDto<LoginResponseDto>.Fail("User not found or inactive.");

            var roles = await _userManager.GetRolesAsync(user);
            var jti = Guid.NewGuid().ToString();

            var newAccessToken = await _jwtTokenService.GenerateAccessTokenAsync(user, roles, jti: jti);
            var newRefreshToken = _jwtTokenService.GenerateRefreshToken();
            var newRefreshTokenHash = _jwtTokenService.HashToken(newRefreshToken);

            storedToken.IsRevoked = true;
            storedToken.RevokedAt = DateTime.UtcNow;
            storedToken.RevokedByIp = GetIpAddress();
            storedToken.ReplacedByTokenHash = newRefreshTokenHash;

            _dbContext.RefreshTokens.Add(new RefreshToken
            {
                UserId = user.Id,
                TokenHash = newRefreshTokenHash,
                JwtId = jti,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                CreatedByIp = GetIpAddress(),
                CreatedBy = user.Email ?? user.UserName ?? string.Empty
            });

            await _dbContext.SaveChangesAsync();

            var permissions = await GetPermissionsForUserAsync(user);

            return ApiResponseDto<LoginResponseDto>.Ok(new LoginResponseDto
            {
                UserId = user.Id,
                Email = user.Email!,
                FullName = user.FullName,
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                AccessTokenExpiresAt = DateTime.UtcNow.AddMinutes(60),
                Roles = roles.ToList(),
                Permissions = permissions.ToList()
            }, "Token refreshed successfully.");
        }

        public async Task<ApiResponseDto<object>> LogoutAsync(RefreshTokenDto request)
            => await RevokeTokenAsync(request);

        public async Task<ApiResponseDto<object>> RevokeTokenAsync(RefreshTokenDto request)
        {
            var tokenHash = _jwtTokenService.HashToken(request.RefreshToken);
            var storedToken = await _dbContext.RefreshTokens.FirstOrDefaultAsync(x => x.TokenHash == tokenHash);

            if (storedToken is null)
                return ApiResponseDto<object>.Fail("Refresh token not found.");

            storedToken.IsRevoked = true;
            storedToken.RevokedAt = DateTime.UtcNow;
            storedToken.RevokedByIp = GetIpAddress();

            await _dbContext.SaveChangesAsync();

            return ApiResponseDto<object>.Ok(null, "Token revoked successfully.");
        }

        public async Task<ApiResponseDto<object>> ForgotPasswordAsync(ForgotPasswordDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
                return ApiResponseDto<object>.Ok(null, "If the email exists, a reset link has been sent.");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            await _emailService.SendAsync(
                user.Email!,
                "Password reset",
                $"Use this token to reset your password: {token}");

            return ApiResponseDto<object>.Ok(null, "If the email exists, a reset link has been sent.");
        }

        public async Task<ApiResponseDto<object>> ResetPasswordAsync(ResetPasswordDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
                return ApiResponseDto<object>.Fail("User not found.");

            var resetResult = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
            return resetResult.Succeeded
                ? ApiResponseDto<object>.Ok(null, "Password reset successful.")
                : ApiResponseDto<object>.Fail("Password reset failed.", resetResult.Errors.Select(x => x.Description).ToArray());
        }

        public async Task<ApiResponseDto<object>> ChangePasswordAsync(Guid userId, ChangePasswordDto request)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user is null)
                return ApiResponseDto<object>.Fail("User not found.");

            var changeResult = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
            return changeResult.Succeeded
                ? ApiResponseDto<object>.Ok(null, "Password changed successfully.")
                : ApiResponseDto<object>.Fail("Password change failed.", changeResult.Errors.Select(x => x.Description).ToArray());
        }

        public async Task<ApiResponseDto<LoginResponseDto>> GetCurrentUserAsync(Guid userId)
        {
       
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user is null)
            return ApiResponseDto<LoginResponseDto>.Fail("User not found.");

        var roles = await _userManager.GetRolesAsync(user);
        var permissions = await GetPermissionsForUserAsync(user);

        return ApiResponseDto<LoginResponseDto>.Ok(new LoginResponseDto
        {
            UserId = user.Id,
            Email = user.Email ?? string.Empty,
            FullName = user.FullName,
            Roles = roles.ToList(),
            Permissions = permissions.ToList()
        }, "Current user retrieved successfully.");
    }

        public async Task<ApiResponseDto<LoginResponseDto>> UpdateProfileAsync(Guid userId, UpdateProfileDto request)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user is null)
                return ApiResponseDto<LoginResponseDto>.Fail("User not found.");

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.ProfileImage = request.ProfileImage;
            user.UpdatedAt = DateTime.UtcNow;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
                return ApiResponseDto<LoginResponseDto>.Fail("Profile update failed.", updateResult.Errors.Select(x => x.Description).ToArray());

            var roles = await _userManager.GetRolesAsync(user);
            var permissions = await GetPermissionsForUserAsync(user);

            return ApiResponseDto<LoginResponseDto>.Ok(new LoginResponseDto
            {
                UserId = user.Id,
                Email = user.Email ?? string.Empty,
                FullName = user.FullName,
                Roles = roles.ToList(),
                Permissions = permissions.ToList()
            }, "Profile updated successfully.");
        }

        private async Task<IEnumerable<string>> GetPermissionsForUserAsync(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var permissions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var roleName in roles)
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role is null) continue;

                var claims = await _roleManager.GetClaimsAsync(role);
                foreach (var claim in claims.Where(x => x.Type == AppConstants.PermissionClaimType))
                {
                    permissions.Add(claim.Value);
                }
            }

            return permissions;
        }

        private string GetIpAddress()
        {
            return _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        }

        private static LoginResponseDto MapUser(ApplicationUser user, IEnumerable<string> roles, IEnumerable<string> permissions)
        {
            return new LoginResponseDto
            {
                UserId = user.Id,
                Email = user.Email ?? string.Empty,
                FullName = $"{user.FirstName} {user.LastName}".Trim(),
                Roles = roles.ToList(),
                Permissions = permissions.ToList()
            };
        }
}

