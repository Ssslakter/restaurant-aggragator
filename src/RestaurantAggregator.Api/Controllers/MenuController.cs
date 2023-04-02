using Microsoft.AspNetCore.Mvc;
using RestaurantAggregator.Core.Data.DTO;
using RestaurantAggregator.Core.Data.Enums;

namespace RestaurantAggregator.Api.Controllers;

[ApiController]
[Route("menu")]
public class MenuController : ControllerBase
{
    [HttpGet("{restaurantId}/all")]
    public Task<ActionResult<ICollection<MenuDTO>>> GetRestaurantMenus(Guid restaurantId)
    {
        throw new NotImplementedException();
    }

    [HttpGet("{menuId}")]
    public Task<ActionResult<MenuDetails>> GetMenu(Guid menuId,
    [FromQuery] ICollection<Category> filters,
    [FromQuery] Sorting sorting,
    [FromQuery] uint page)
    {
        throw new NotImplementedException();
    }
    //все эндпоинты ниже выполняются для меню ресторана для конкретного менеджера
    [HttpPost("create")]
    public Task<IActionResult> CreateMenu(MenuCreation menu)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("{menuId}/delete")]
    public Task<IActionResult> DeleteMenu(Guid menuId)
    {
        throw new NotImplementedException();
    }

    [HttpPut("{menuId}/edit")]
    public Task<IActionResult> EditMenu(Guid menuId, MenuCreation menu)
    {
        throw new NotImplementedException();
    }

    [HttpPost("{menuId}/dish/add")]
    public Task<IActionResult> AddDishToMenu(Guid menuId,
     [FromQuery] Guid dishId)
    {
        throw new NotImplementedException();
    }

    [HttpPost("{menuId}/dish/remove")]
    public Task<IActionResult> RemoveDishFromMenu(Guid menuId,
     [FromQuery] Guid dishId)
    {
        throw new NotImplementedException();
    }
}
