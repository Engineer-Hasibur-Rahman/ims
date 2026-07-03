using ims.Application.DTOs;
using ims.Application.Interfaces;
using ims.Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ims.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

public class BrandsController : ControllerBase
{
    private readonly IBrandService _brandService;

    public BrandsController(IBrandService brandService)
    {
        _brandService = brandService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BrandResponseDto>>> GetAll(CancellationToken cancellationToken)
    {
        var brands = await _brandService.GetAllAsync(cancellationToken);
        return Ok(brands);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<BrandResponseDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var brand = await _brandService.GetByIdAsync(id, cancellationToken);
        if (brand is null)
            return NotFound(new { message = "Brand not found." });
        return Ok(brand);
    }

    [HttpPost]
    public async Task<ActionResult<BrandResponseDto>> Create([FromBody] BrandCreateDto dto, CancellationToken cancellationToken)
    {
        var createdBrand = await _brandService.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = createdBrand.Id }, createdBrand);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<BrandResponseDto>> Update(int id, [FromBody] BrandUpdateDto dto, CancellationToken cancellationToken)
    {

        try
        {
            var updated = await _brandService.UpdateAsync(id, dto, cancellationToken);

            if (!updated)
                return NotFound(new { message = "Category not found." });

            return Ok(new { message = "Category updated successfully." });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await _brandService.DeleteAsync(id, cancellationToken);
        if (!deleted)
            return NotFound(new { message = "Brand not found." });
        return NoContent();
    }



    private ActionResult<IEnumerable<BrandResponseDto>> Ok(IEnumerable<BrandResponseDto> brands)
    {
        throw new NotImplementedException();
    }
}
