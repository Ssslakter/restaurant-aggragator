using Microsoft.AspNetCore.Mvc;
using RestaurantAggregator.Auth.Services;
using RestaurantAggregator.Core.Data.DTO;
using RestaurantAggregator.Core.Data.Enums;
using RestaurantAggregator.Infra.Auth;

namespace RestaurantAggregator.Auth.Controllers;

[ApiController]
[RoleAuthorize(RoleType.Admin)]
[Route("api/admin")]
public class AdminController : AuthControllerBase
{
    private readonly IProfileService _profileService;
    private readonly IRolesService _rolesService;

    public AdminController(IProfileService profileService, IRolesService rolesService)
    {
        _profileService = profileService;
        _rolesService = rolesService;
    }

    [HttpGet("users")]
    public async Task<ActionResult> GetUsers([FromQuery] uint page)
    {
        return Ok(await _profileService.GetUserProfilesAsync(page));
    }

    [HttpGet("users/{userId}/roles")]
    public async Task<ActionResult<IEnumerable<RoleType>>> GetUserRoles(Guid userId)
    {
        return Ok(await _rolesService.GetUserRolesAsync(userId));
    }

    [HttpPost("users/{userId}/roles")]
    public async Task<IActionResult> GiveUserRole(Guid userId, [FromQuery] RoleType roleType)
    {
        await _rolesService.AddRoleToUserAsync(userId, roleType);
        return Ok();
    }

    [HttpDelete("users/{userId}/roles")]
    public async Task<IActionResult> RevokeUserRole(Guid userId, [FromQuery] RoleType roleType)
    {
        await _rolesService.RemoveRoleFromUserAsync(userId, roleType);
        return Ok();
    }

    [HttpGet("users/{userId}")]
    public async Task<ActionResult<ProfileDTO>> GetProfile(Guid userId)
    {
        return Ok(await _profileService.GetProfileAsync(userId));
    }

    [HttpDelete("users/{userId}")]
    public async Task<IActionResult> DeleteUser(Guid userId)
    {
        await _profileService.DeleteUserAsync(userId);
        return Ok();
    }
}
