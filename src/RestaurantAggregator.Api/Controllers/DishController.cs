using Microsoft.AspNetCore.Mvc;
using RestaurantAggregator.Core.Data.DTO;
using RestaurantAggregator.Core.Data.Enums;
using RestaurantAggregator.Core.Services;
using RestaurantAggregator.Infra.Auth;

namespace RestaurantAggregator.Api.Controllers;

[ApiController]
[Route("api/restaurant")]
public class DishController : AuthControllerBase
{
    private readonly IDishService _dishService;
    private readonly IPermissionService _permissionService;

    public DishController(IDishService dishService, IPermissionService permissionService)
    {
        _dishService = dishService;
        _permissionService = permissionService;
    }

    [HttpGet("{restaurantId}/dish/{dishId}")]
    public async Task<ActionResult<DishDTO>> GetDishInfo(Guid restaurantId, Guid dishId)
    {
        var dish = await _dishService.GetDishInfoByIdAsync(dishId, restaurantId);
        return Ok(dish);
    }

    [HttpPost("{restaurantId}/dish/{dishId}/review")]
    [RoleAuthorize(RoleType.Client)]
    public async Task<IActionResult> AddDishReview(Guid dishId, ReviewDTO reviewModel)
    {
        await _dishService.AddReviewToDishAsync(dishId, UserId, reviewModel);
        return Ok();
    }

    [HttpPost("{restaurantId}/dish")]
    [RoleAuthorize(RoleType.Manager)]
    public async Task<IActionResult> CreateDish(Guid restaurantId, DishCreation dishModel)
    {
        await _permissionService.RestaurantOwnerValidate(UserId, restaurantId);
        await _dishService.CreateDishAsync(dishModel, restaurantId);
        return Ok();
    }

    [HttpPut("{restaurantId}/dish/{dishId}")]
    [RoleAuthorize(RoleType.Manager)]
    public async Task<ActionResult<DishDTO>> UpdateDish(Guid restaurantId, Guid dishId, DishCreation dishModel)
    {
        await _permissionService.RestaurantOwnerValidate(UserId, restaurantId);
        var dish = await _dishService.UpdateDishAsync(dishModel, dishId, restaurantId);
        return Ok(dish);
    }

    [HttpDelete("{restaurantId}/dish/{dishId}")]
    [RoleAuthorize(RoleType.Manager)]
    public async Task<IActionResult> DeleteDish(Guid restaurantId, Guid dishId)
    {
        await _permissionService.RestaurantOwnerValidate(UserId, restaurantId);
        await _dishService.DeleteDishAsync(dishId, restaurantId);
        return Ok();
    }
}
