using ims.Application.DTOs;

namespace ims.Application.Interfaces
{
    public class IUserService
    {
        Task<ApiResponseDto<object>> AssignRoleAsync(Guid userId, string roleName);
        Task<ApiResponseDto<object>> RemoveRoleAsync(Guid userId, string roleName);
    }
}
