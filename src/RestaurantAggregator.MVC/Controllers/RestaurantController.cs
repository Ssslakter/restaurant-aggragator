using Microsoft.AspNetCore.Mvc;
using RestaurantAggregator.Core.Data.DTO;
using RestaurantAggregator.Core.Data.Enums;
using RestaurantAggregator.Core.Services;
using RestaurantAggregator.Infra.Auth;

namespace RestaurantAggregator.MVC.Controllers;

[Route("admin/restaurant")]
[RoleAuthorize(RoleType.Admin)]
public class RestaurantController : Controller
{
    private readonly IRestaurantService _restaurantService;
    private readonly IConfiguration _config;
    private readonly IPermissionService _permissionService;

    public RestaurantController(IRestaurantService restaurantService,
     IConfiguration config, IPermissionService permissionService)
    {
        _restaurantService = restaurantService;
        _config = config;
        _permissionService = permissionService;
    }
    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] int page)
    {
        ViewData["Page"] = page == 0 ? 1 : page;
        var restaurants = await _restaurantService.GetRestaurantsAsync((uint)page);
        return View(restaurants);
    }
    [HttpGet("{restaurantId}")]
    public async Task<IActionResult> Staff(Guid restaurantId, string restaurantName)
    {
        var staff = await _restaurantService.GetRestaurantStaffAsync(restaurantId);
        ViewData["Name"] = restaurantName;
        ViewData["RestaurantId"] = restaurantId;
        ViewData["AuthApiUrl"] = _config["AuthApiUrl"];
        return View(staff);
    }

    [HttpPost("{restaurantId}/staff/add")]
    public async Task<IActionResult> AddStaff(Guid restaurantId, Guid userId, RoleType role)
    {
        await _permissionService.GivePermissionToUser(userId, restaurantId, role);
        return LocalRedirect($"/admin/restaurant/{restaurantId}");
    }

    [HttpPost("{restaurantId}/staff/delete")]
    public async Task<IActionResult> RemoveStaff(Guid restaurantId, Guid userId, RoleType role)
    {
        await _permissionService.RevokePermissionFromUser(userId, restaurantId, role);
        return LocalRedirect($"/admin/restaurant/{restaurantId}");
    }

    [HttpPost]
    public async Task<IActionResult> CreateRestaurant(RestaurantCreation restaurantModel)
    {
        await _restaurantService.CreateRestaurantAsync(restaurantModel);
        return LocalRedirect("/admin/restaurant/");
    }

    [HttpPost("{restaurantId}/put")]
    public async Task<IActionResult> UpdateRestaurant(Guid restaurantId, RestaurantCreation restaurantModel)
    {
        await _restaurantService.UpdateRestaurantAsync(restaurantId, restaurantModel);
        return LocalRedirect("/admin/restaurant/");
    }

    [HttpPost("{restaurantId}/delete")]
    public async Task<IActionResult> DeleteRestaurant(Guid restaurantId)
    {
        await _restaurantService.DeleteRestaurantAsync(restaurantId);
        return LocalRedirect("/admin/restaurant/");
    }
}
