using ims.Application.DTOs;
using ims.Application.Interfaces;
using ims.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ims.Application.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public RoleService(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<ApiResponseDto<IEnumerable<string>>> GetAllRolesAsync()
        {
            var roles = await _roleManager.Roles
                .Where(x => x.Name != null)
                .OrderBy(x => x.Name)
                .Select(x => x.Name!)
                .ToListAsync();

            return ApiResponseDto<IEnumerable<string>>.Ok(
                roles,
                "Roles retrieved successfully."
            );
        }
    }
}