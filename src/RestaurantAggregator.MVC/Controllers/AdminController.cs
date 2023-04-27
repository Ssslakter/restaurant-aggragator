using Microsoft.AspNetCore.Mvc;
using RestaurantAggregator.Core.Data.Enums;

namespace RestaurantAggregator.MVC.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdminController : Controller
{
    private readonly IRolesService _rolesService;

    public AdminController(IRolesService rolesService)
    {
        _rolesService = rolesService;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Users()
    {
        //get list of all users from api
        return View();
    }

    [HttpGet]
    public IActionResult Restaurants()
    {
        //get list of all restaurants from api
        return View();
    }

    [HttpGet]
    public IActionResult Users(Guid userId)
    {
        //get user by id from api
        return View();
    }

    [HttpPost]
    public IActionResult GiveRole(Guid userId, RoleType role)
    {
        return Ok();
    }

    [HttpPost]
    public IActionResult RemoveRole(Guid userId, RoleType role)
    {
        return Ok();
    }
}
