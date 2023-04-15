using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantAggregator.Core.Data.DTO;
using RestaurantAggregator.Core.Services;

namespace RestaurantAggregator.Api.Controllers;

[ApiController]
[Route("dish")]
public class DishController : ControllerBase
{
    private readonly IDishService _dishService;

    public DishController(IDishService dishService)
    {
        _dishService = dishService;
    }

    [HttpGet("{dishId}")]
    public async Task<ActionResult<DishDTO>> GetDishInfo(Guid dishId)
    {
        var dish = await _dishService.GetDishInfoByIdAsync(dishId);
        return Ok(dish);
    }

    [Authorize(Roles = "Manager")]
    [HttpPost("create")]
    public async Task<IActionResult> CreateDish(DishCreation dish, [FromQuery] Guid menuId)
    {
        await _dishService.CreateDishAsync(dish, menuId);
        return Ok();
    }

    [Authorize(Roles = "Manager")]
    [HttpPut("{dishId}/edit")]
    public async Task<IActionResult> EditDish(Guid dishId, DishCreation dish)
    {
        await _dishService.UpdateDishAsync(dish, dishId);
        return Ok();
    }

    [Authorize(Roles = "Manager")]
    [HttpDelete("{dishId}/delete")]
    public async Task<IActionResult> DeleteDish(Guid dishId)
    {
        await _dishService.DeleteDishAsync(dishId);
        return Ok();
    }

    [Authorize(Roles = "Client")]
    [HttpPost("{dishId}/review")]
    public async Task<IActionResult> AddDishReview(Guid dishId, ReviewDTO reviewModel)
    {
#nullable disable
        var clientId = Guid.Parse((User.Identity as ClaimsIdentity)?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
#nullable enable
        await _dishService.AddReviewToDishAsync(dishId, clientId, reviewModel);
        return Ok();
    }
}
