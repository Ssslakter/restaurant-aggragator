using Microsoft.AspNetCore.Mvc;
using RestaurantAggregator.Auth.BL.Services;
using RestaurantAggregator.Core.Data.DTO;
using RestaurantAggregator.Core.Data.Enums;
using RestaurantAggregator.Infra.Auth;
using RestaurantAggregator.MVC.Models;

namespace RestaurantAggregator.MVC.Controllers;

//[RoleAuthorize(RoleType.Admin)]
[Route("admin/users")]
public class UsersController : Controller
{
    private readonly IRolesService _rolesService;
    private readonly IProfileService _usersService;

    public UsersController(IRolesService rolesService, IProfileService usersService)
    {
        _rolesService = rolesService;
        _usersService = usersService;
    }

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] uint page)
    {
        var users = await _usersService.GetUserProfilesAsync(page);
        return View(users);
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> UserInfo(Guid userId)
    {
        var user = await _usersService.GetProfileAsync(userId);
        var roles = await _rolesService.GetUserRolesAsync(userId);
        var userDTO = ProfileWithRolesDTO.FromProfileDTO(user);
        userDTO.Roles = roles;
        return View(userDTO);
    }

    [HttpPost("{userId}/delete")]
    public async Task<IActionResult> RemoveRole(Guid userId, RoleType role)
    {
        await _rolesService.RemoveRoleFromUserAsync(userId, role);
        return RedirectToPage($"/admin/users/{userId}");
    }

    [HttpPost("{userId}")]
    public async Task<IActionResult> AddRole(Guid userId, RoleType role)
    {
        await _rolesService.AddRoleToUserAsync(userId, role);
        return LocalRedirectPermanent($"/admin/users/{userId}");
    }
}
