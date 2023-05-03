using Microsoft.AspNetCore.Mvc;
using RestaurantAggregator.Auth.BL.Services;
using RestaurantAggregator.Core.Data.DTO;
using RestaurantAggregator.Core.Data.Enums;
using RestaurantAggregator.Core.Services;

namespace RestaurantAggregator.MVC.Controllers;

[Route("admin/restaurant")]
public class RestaurantController : Controller
{
    private readonly IRestaurantService _restaurantService;
    private readonly IProfileService _usersService;
    private readonly IPermissionService _permissionService;

    public RestaurantController(IRestaurantService restaurantService,
     IProfileService usersService, IPermissionService permissionService)
    {
        _restaurantService = restaurantService;
        _usersService = usersService;
        _permissionService = permissionService;
    }
    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] uint page)
    {
        var restaurants = await _restaurantService.GetRestaurantsAsync(page);
        ViewData["url"] = Request.Path.Value;
        ViewData["Message"] = "shit";
        return View(restaurants);
    }
    [HttpGet("{restaurantId}")]
    public async Task<IActionResult> Staff(Guid restaurantId, string restaurantName)
    {
        var staff = await _restaurantService.GetRestaurantStaffAsync(restaurantId);
        ViewData["Name"] = restaurantName;
        ViewData["RestaurantId"] = restaurantId;
        return View(staff);
    }

    [HttpPost("{restaurantId}/staff/add")]
    public async Task<IActionResult> AddStaff(Guid restaurantId, [FromQuery] Guid userId, [FromQuery] RoleType role)
    {
        await _permissionService.GivePermissionToUser(userId, restaurantId, role);
        return RedirectToPage($"/admin/restaurant/{restaurantId}");
    }

    [HttpPost("{restaurantId}/staff/delete")]
    public async Task<IActionResult> RemoveStaff(Guid restaurantId, [FromQuery] Guid userId, [FromQuery] RoleType role)
    {
        await _permissionService.RevokePermissionFromUser(userId, restaurantId, role);
        return RedirectToPage($"/admin/restaurant/{restaurantId}");
    }

    [HttpPost]
    public async Task<IActionResult> CreateRestaurant(RestaurantCreation restaurantModel)
    {
        await _restaurantService.CreateRestaurantAsync(restaurantModel);
        return RedirectToPage("/admin/restaurant/");
    }

    [HttpPost("{restaurantId}/put")]
    public async Task<IActionResult> UpdateRestaurant(Guid restaurantId, RestaurantCreation restaurantModel)
    {
        await _restaurantService.UpdateRestaurantAsync(restaurantId, restaurantModel);
        return RedirectToPage("/admin/restaurant/");
    }

    [HttpPost("{restaurantId}/delete")]
    public async Task<IActionResult> DeleteRestaurant(Guid restaurantId)
    {
        await _restaurantService.DeleteRestaurantAsync(restaurantId);
        return RedirectToPage("/admin/restaurant/");
    }
}
