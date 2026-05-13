using ims.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ims.API.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("{userId:guid}/roles/{roleName}")]
    public async Task<IActionResult> AssignRole(Guid userId, string roleName)
    {
        var result = await _userService.AssignRoleAsync(userId, roleName);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{userId:guid}/roles/{roleName}")]
    public async Task<IActionResult> RemoveRole(Guid userId, string roleName)
    {
        var result = await _userService.RemoveRoleAsync(userId, roleName);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}
