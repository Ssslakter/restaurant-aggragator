using Microsoft.AspNetCore.Mvc;

namespace RestaurantAggregator.Api.Controllers;

[ApiController]
[Route("menu")]
public class MenuController : ControllerBase
{
    [HttpGet("{restaurantId}/all")]
    public Task<IActionResult> GetRestaurantMenus(string restaurantId)
    {
        throw new NotImplementedException();
    }

    [HttpGet("{menuId}")]
    public Task<IActionResult> GetMenu(string menuId,
    [FromQuery] string filters,
    [FromQuery] string sorting,
    [FromQuery] uint page)
    {
        throw new NotImplementedException();
    }
    //все эндпоинты ниже выполняются для меню ресторана для конкретного менеджера
    [HttpPost("create")]
    public Task<IActionResult> CreateMenu(string name)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("{menuId}/delete")]
    public Task<IActionResult> DeleteMenu(string menuId)
    {
        throw new NotImplementedException();
    }

    [HttpPatch("{menuId}/edit")]
    public Task<IActionResult> EditMenu(string menuId,
     [FromQuery] string name)
    {
        throw new NotImplementedException();
    }

    [HttpPost("{menuId}/dish/add")]
    public Task<IActionResult> AddDishToMenu(string menuId,
     [FromQuery] string dishId)
    {
        throw new NotImplementedException();
    }

    [HttpPost("{menuId}/dish/remove")]
    public Task<IActionResult> RemoveDishFromMenu(string menuId,
     [FromQuery] string dishId)
    {
        throw new NotImplementedException();
    }
}
