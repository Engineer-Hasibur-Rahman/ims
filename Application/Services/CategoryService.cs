using ims.Application.DTOs;
using ims.Application.Interfaces;
using ims.Domain.Entities;
using ims.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ims.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly AppDbContext _context;

    public CategoryService(AppDbContext context)
    {
        _context = context;
    }

    // ================= GET ALL =================
    public async Task<IEnumerable<CategoryResponseDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Categories
            .AsNoTracking()
            .Where(x => !x.IsDeleted)
            .OrderByDescending(x => x.Id)
            .Select(x => new CategoryResponseDto
            {
                Id = x.Id,
                Name = x.Name,
                Slug = x.Slug,
                Description = x.Description,
                ParentId = x.ParentId,
                IsActive = x.IsActive,
                CreatedAt = x.CreatedAt
            })
            .ToListAsync(cancellationToken);
    }

    // ================= GET BY ID =================
    public async Task<CategoryResponseDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Categories
            .AsNoTracking()
            .Where(x => x.Id == id && !x.IsDeleted)
            .Select(x => new CategoryResponseDto
            {
                Id = x.Id,
                Name = x.Name,
                Slug = x.Slug,
                Description = x.Description,
                ParentId = x.ParentId,
                IsActive = x.IsActive,
                CreatedAt = x.CreatedAt
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    // ================= CREATE =================
    public async Task<CategoryResponseDto> CreateAsync(CategoryCreateDto dto, CancellationToken cancellationToken = default)
    {
        var name = dto.Name.Trim();

        var exists = await _context.Categories
            .AnyAsync(x => x.Name == name && !x.IsDeleted, cancellationToken);

        if (exists)
            throw new InvalidOperationException("Category name already exists.");

        var entity = new Category
        {
            Name = name,
            Slug = GenerateSlug(name),
            Description = dto.Description?.Trim(),
            ParentId = dto.ParentId,
            IsActive = dto.IsActive,
            Media = dto.Media ?? string.Empty,
            CreatedAt = DateTime.UtcNow
        };

        _context.Categories.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return Map(entity);
    }

    // ================= UPDATE =================
    public async Task<bool> UpdateAsync(int id, CategoryUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _context.Categories
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken);

        if (entity is null)
            return false;

        var name = dto.Name.Trim();

        var duplicate = await _context.Categories
            .AnyAsync(x => x.Id != id && x.Name == name && !x.IsDeleted, cancellationToken);

        if (duplicate)
            throw new InvalidOperationException("Category name already exists.");

        entity.Name = name;
        entity.Slug = GenerateSlug(name);
        entity.Description = dto.Description?.Trim();
        entity.ParentId = dto.ParentId;
        entity.IsActive = dto.IsActive;
        entity.Media = dto.Media ?? entity.Media;
        entity.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    // ================= DELETE (SOFT) =================
    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.Categories
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken);

        if (entity is null)
            return false;

        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    // ================= HELPERS =================
    private static string GenerateSlug(string name)
    {
        return name.ToLower()
            .Replace(" ", "-")
            .Replace("--", "-");
    }

    private static CategoryResponseDto Map(Category entity)
    {
        return new CategoryResponseDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Slug = entity.Slug,
            Description = entity.Description,
            ParentId = entity.ParentId,
            IsActive = entity.IsActive,
            CreatedAt = entity.CreatedAt
        };
    }
}