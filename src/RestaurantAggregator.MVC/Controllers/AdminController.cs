using Microsoft.AspNetCore.Mvc;
using RestaurantAggregator.Auth.BL.Services;
using RestaurantAggregator.Core.Data.Enums;
using RestaurantAggregator.Infra.Auth;

namespace RestaurantAggregator.MVC.Controllers;

//[RoleAuthorize(RoleType.Admin)]
[Route("users")]
public class AdminController : Controller
{
    private readonly IRolesService _rolesService;
    private readonly IProfileService _usersService;

    public AdminController(IRolesService rolesService, IProfileService usersService)
    {
        _rolesService = rolesService;
        _usersService = usersService;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("users")]
    public async Task<IActionResult> Users([FromQuery] uint page)
    {
        var users = await _usersService.GetUserProfilesAsync(page);
        return View(users);
    }

    public async Task<IActionResult> Restaurants()
    {
        return View();
    }

    public IActionResult Users(Guid userId)
    {
        //get user by id from api
        return View();
    }

    public IActionResult GiveRole(Guid userId, RoleType role)
    {
        return Ok();
    }

    public IActionResult RemoveRole(Guid userId, RoleType role)
    {
        return Ok();
    }
}
