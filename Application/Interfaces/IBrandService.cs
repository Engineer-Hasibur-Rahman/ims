using ims.Application.DTOs;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace ims.Application.Interfaces
{
    public interface IBrandService
    {
        Task <IEnumerable<BrandResponseDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<BrandResponseDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
       Task<BrandResponseDto> CreateAsync(BrandCreateDto dto, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(int id, BrandUpdateDto dto, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
