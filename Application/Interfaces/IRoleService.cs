using ims.Application.DTOs;

namespace ims.Application.Interfaces;

public interface IRoleService
{
    Task<ApiResponseDto<IEnumerable<string>>> GetAllRolesAsync();
}