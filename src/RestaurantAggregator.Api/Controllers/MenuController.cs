using Microsoft.AspNetCore.Mvc;
using RestaurantAggregator.Core.Data.DTO;
using RestaurantAggregator.Core.Data.Enums;
using RestaurantAggregator.Core.Services;

namespace RestaurantAggregator.Api.Controllers;

[ApiController]
[Route("menu")]
public class MenuController : ControllerBase
{
    private readonly IMenuService _menuService;

    public MenuController(IMenuService menuService)
    {
        _menuService = menuService;
    }

    [HttpGet("{restaurantId}/all")]
    public async Task<ActionResult<ICollection<MenuDTO>>> GetRestaurantMenus(Guid restaurantId)
    {
        return Ok(await _menuService.GetMenusByRestaurantIdAsync(restaurantId));
    }

    [HttpGet("{menuId}")]
    public async Task<ActionResult<MenuDetails>> GetMenu(Guid menuId,
    [FromQuery] ICollection<Category> filters,
    [FromQuery] Sorting sorting,
    [FromQuery] uint page = 1)
    {
        var menu = await _menuService.GetMenuByIdAsync(menuId, filters, sorting, page);
        return Ok(menu);
    }
}
