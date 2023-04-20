using Microsoft.AspNetCore.Mvc;
using RestaurantAggregator.Core.Data.DTO;
using RestaurantAggregator.Core.Data.Enums;
using RestaurantAggregator.Core.Services;
using RestaurantAggregator.Infra.Auth;

namespace RestaurantAggregator.Api.Controllers;

[ApiController]
[Route("menu")]
public class MenuController : AuthControllerBase
{
    private readonly IMenuService _menuService;
    private readonly IPermissionService _permissionService;

    public MenuController(IMenuService menuService, IPermissionService permissionService)
    {
        _menuService = menuService;
        _permissionService = permissionService;
    }

    [HttpGet("{restaurantId}/all")]
    public async Task<ActionResult<ICollection<MenuDTO>>> GetRestaurantMenus(Guid restaurantId)
    {
        return Ok(await _menuService.GetMenusByRestaurantIdAsync(restaurantId));
    }

    [HttpGet("{menuId}")]
    public async Task<ActionResult<MenuDetails>> GetMenuDetails(Guid menuId,
    [FromQuery] ICollection<Category> filters,
    [FromQuery] Sorting sorting,
    [FromQuery] uint page = 1)
    {
        var menu = await _menuService.GetMenuByIdAsync(menuId, filters, sorting, page);
        return Ok(menu);
    }

    [HttpPost("{restaurantId}")]
    [RoleAuthorize(RoleType.Manager)]
    public async Task<ActionResult<MenuDTO>> CreateMenu(Guid restaurantId, MenuCreation menuModel)
    {
        await _permissionService.RestaurantOwnerValidate(UserId, restaurantId);
        var menu = await _menuService.CreateMenuAsync(menuModel, restaurantId);
        return Ok(menu);
    }

    [HttpPut("{menuId}")]
    [RoleAuthorize(RoleType.Manager)]
    public async Task<ActionResult<MenuDTO>> UpdateMenu(Guid menuId, MenuCreation menuModel)
    {
        await _permissionService.MenuOwnerValidate(UserId, menuId);
        var menu = await _menuService.UpdateMenuAsync(menuModel, menuId);
        return Ok(menu);
    }

    [HttpDelete("{menuId}")]
    [RoleAuthorize(RoleType.Manager)]
    public async Task<IActionResult> DeleteMenu(Guid menuId)
    {
        await _permissionService.MenuOwnerValidate(UserId, menuId);
        await _menuService.DeleteMenuAsync(menuId);
        return Ok();
    }

    [HttpPost("{menuId}/dish/{dishId}/add")]
    [RoleAuthorize(RoleType.Manager)]
    public async Task<IActionResult> AddDishToMenu(Guid menuId, Guid dishId)
    {
        await _permissionService.MenuOwnerValidate(UserId, menuId);
        await _menuService.AddDishToMenuAsync(menuId, dishId);
        return Ok();
    }

    [HttpPost("{menuId}/dish/{dishId}/remove")]
    [RoleAuthorize(RoleType.Manager)]
    public async Task<IActionResult> RemoveDishFromMenu(Guid menuId, Guid dishId)
    {
        await _permissionService.MenuOwnerValidate(UserId, menuId);
        await _menuService.RemoveDishFromMenuAsync(menuId, dishId);
        return Ok();
    }
}
