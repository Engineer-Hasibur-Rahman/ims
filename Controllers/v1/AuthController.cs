using ims.Application.DTOs;
using ims.Application.DTOs.Auth;
using ims.Application.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static ims.Shared.Constants.Permissions;

namespace ims.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]  
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public Task<ApiResponseDto<LoginResponseDto>> Register([FromBody] RegisterRequestDto request)
            => _authService.RegisterAsync(request);

        [HttpPost("login")]
        [AllowAnonymous]
        public Task<ApiResponseDto<LoginResponseDto>> Login([FromBody] LoginRequestDto request)
            => _authService.LoginAsync(request);

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public Task<ApiResponseDto<LoginResponseDto>> RefreshToken([FromBody] RefreshTokenDto request)
            => _authService.RefreshTokenAsync(request);

        [HttpPost("logout")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public Task<ApiResponseDto<object>> Logout([FromBody] RefreshTokenDto request)
            => _authService.LogoutAsync(request);

        [HttpPost("revoke-token")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public Task<ApiResponseDto<object>> RevokeToken([FromBody] RefreshTokenDto request)
            => _authService.RevokeTokenAsync(request);

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public Task<ApiResponseDto<object>> ForgotPassword([FromBody] ForgotPasswordDto request)
            => _authService.ForgotPasswordAsync(request);

        [HttpPost("reset-password")]
        [AllowAnonymous]
        public Task<ApiResponseDto<object>> ResetPassword([FromBody] ResetPasswordDto request)
            => _authService.ResetPasswordAsync(request);

        [HttpPost("change-password")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public Task<ApiResponseDto<object>> ChangePassword([FromBody] ChangePasswordDto request)
            => _authService.ChangePasswordAsync(GetUserId(), request);

        [HttpGet("me")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public Task<ApiResponseDto<LoginResponseDto>> Me()
            => _authService.GetCurrentUserAsync(GetUserId());


        [HttpPut("profile")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public Task<ApiResponseDto<LoginResponseDto>> UpdateProfile([FromBody] UpdateProfileDto request)
            => _authService.UpdateProfileAsync(GetUserId(), request);

        private Guid GetUserId()
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.Parse(id!);
        }
    }
}
