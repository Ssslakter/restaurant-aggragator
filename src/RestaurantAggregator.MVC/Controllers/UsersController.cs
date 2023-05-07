using Microsoft.AspNetCore.Mvc;
using RestaurantAggregator.Auth.BL.Services;
using RestaurantAggregator.Core.Data.DTO;
using RestaurantAggregator.Core.Data.Enums;
using RestaurantAggregator.Core.Exceptions;

namespace RestaurantAggregator.MVC.Controllers;

//[RoleAuthorize(RoleType.Admin)]
[Route("admin/users")]
public class UsersController : Controller
{
    private readonly IRolesService _rolesService;
    private readonly IProfileService _usersService;
    private readonly IBanService _banService;

    public UsersController(IRolesService rolesService, IProfileService usersService, IBanService banService)
    {
        _rolesService = rolesService;
        _usersService = usersService;
        _banService = banService;
    }

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] int page)
    {
        ViewData["Page"] = page == 0 ? 1 : page;
        var users = await _usersService.GetUserProfilesAsync((uint)page);
        return View(users);
    }

    [HttpGet("json")]
    public async Task<IActionResult> IndexJson([FromQuery] int page)
    {
        var users = await _usersService.GetUserProfilesAsync((uint)page);
        return Ok(users);
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> UserInfo(Guid userId)
    {
        var user = await _usersService.GetProfileAsync(userId);
        var roles = await _rolesService.GetUserRolesAsync(userId);
        var userDTO = ProfileWithRolesDTO.FromProfileDTO(user);
        ViewData["IsBanned"] = await _banService.IsBannedAsync(userId);
        userDTO.Roles = roles;
        return View(userDTO);
    }

    [HttpPost("{userId}/delete")]
    public async Task<IActionResult> DeleteUser(Guid userId)
    {
        await _usersService.DeleteUserAsync(userId);
        return LocalRedirect("/admin/users");
    }

    [HttpPost("{userId}/ban")]
    public async Task<IActionResult> BanUser(Guid userId)
    {
        await _banService.BanUserAsync(userId);
        return LocalRedirect($"/admin/users/{userId}");
    }

    [HttpPost("{userId}/unban")]
    public async Task<IActionResult> UnbanUser(Guid userId)
    {
        await _banService.UnbanUserAsync(userId);
        return LocalRedirect($"/admin/users/{userId}");
    }

    [HttpPost("{userId}/delete-role")]
    public async Task<IActionResult> RemoveRole(Guid userId, RoleType role)
    {
        if (role == RoleType.Admin || role == RoleType.Client)
            throw new BusinessException("Cannot operate with this role");
        await _rolesService.RemoveRoleFromUserAsync(userId, role);
        return LocalRedirect($"/admin/users/{userId}");
    }

    [HttpPost("{userId}")]
    public async Task<IActionResult> AddRole(Guid userId, RoleType role)
    {
        if (role == RoleType.Admin || role == RoleType.Client)
            throw new BusinessException("Cannot operate with this role");
        await _rolesService.AddRoleToUserAsync(userId, role);
        return LocalRedirect($"/admin/users/{userId}");
    }
}
