using ims.Application.DTOs;
using ims.Application.DTOs.Auth;

namespace ims.Application.Interfaces;

public interface IAuthService
{
    Task<ApiResponseDto<LoginResponseDto>> RegisterAsync(RegisterRequestDto request);
    Task<ApiResponseDto<LoginResponseDto>> LoginAsync(LoginRequestDto request);
    Task<ApiResponseDto<LoginResponseDto>> RefreshTokenAsync(RefreshTokenDto request);
    Task<ApiResponseDto<object>> LogoutAsync(RefreshTokenDto request);
    Task<ApiResponseDto<object>> RevokeTokenAsync(RefreshTokenDto request);
    Task<ApiResponseDto<object>> ForgotPasswordAsync(ForgotPasswordDto request);
    Task<ApiResponseDto<object>> ResetPasswordAsync(ResetPasswordDto request);
    Task<ApiResponseDto<object>> ChangePasswordAsync(Guid userId, ChangePasswordDto request);
    Task<ApiResponseDto<LoginResponseDto>> GetCurrentUserAsync(Guid userId);
    Task<ApiResponseDto<LoginResponseDto>> UpdateProfileAsync(Guid userId, UpdateProfileDto request);
}