using ims.Application.DTOs;
using ims.Application.Interfaces;
using ims.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace ims.Application.Services;

    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ApiResponseDto<object>> AssignRoleAsync(Guid userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user is null)
                return ApiResponseDto<object>.Fail("User not found.");

            var result = await _userManager.AddToRoleAsync(user, roleName);
            return result.Succeeded
                ? ApiResponseDto<object>.Ok(null, "Role assigned successfully.")
                : ApiResponseDto<object>.Fail("Failed to assign role.", result.Errors.Select(x => x.Description).ToArray());
        }

        public async Task<ApiResponseDto<object>> RemoveRoleAsync(Guid userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user is null)
                return ApiResponseDto<object>.Fail("User not found.");

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            return result.Succeeded
                ? ApiResponseDto<object>.Ok(null, "Role removed successfully.")
                : ApiResponseDto<object>.Fail("Failed to remove role.", result.Errors.Select(x => x.Description).ToArray());
        }
    }

