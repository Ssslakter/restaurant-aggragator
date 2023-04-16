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

    [HttpPut("{dishId}")]
    public async Task<IActionResult> UpdateDish(Guid dishId, DishCreation dishModel)
    {
        await _dishService.UpdateDishAsync(dishModel, dishId);
        return Ok();
    }
}
