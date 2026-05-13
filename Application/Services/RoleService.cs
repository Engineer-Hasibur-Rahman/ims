using ims.Application.DTOs;
using ims.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace ims.Application.Services
{
    public class RoleService
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public RoleService(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<ApiResponseDto<IEnumerable<string>>> GetAllRolesAsync()
        {
            var roles = _roleManager.Roles.Select(x => x.Name ?? string.Empty).ToList();
            return ApiResponseDto<IEnumerable<string>>.Ok(roles, "Roles retrieved successfully.");
        }
    }
}
