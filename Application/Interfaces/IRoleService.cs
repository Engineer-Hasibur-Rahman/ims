using ims.Application.DTOs;

namespace ims.Application.Interfaces
{
    public class IRoleService
    {
        Task<ApiResponseDto<IEnumerable<string>>> GetAllRolesAsync();
    }
}
