using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantAggregator.Core.Data.DTO;
using RestaurantAggregator.Core.Data.Enums;
using RestaurantAggregator.Core.Services;
using RestaurantAggregator.Infra.Auth;

namespace RestaurantAggregator.Api.Controllers;

[ApiController]
[Route("dish")]
public class DishController : AuthControllerBase
{
    private readonly IDishService _dishService;
    private readonly IPermissionService _permissionService;

    public DishController(IDishService dishService, IPermissionService permissionService)
    {
        _dishService = dishService;
        _permissionService = permissionService;
    }

    [HttpGet("{dishId}")]
    public async Task<ActionResult<DishDTO>> GetDishInfo(Guid dishId)
    {
        var dish = await _dishService.GetDishInfoByIdAsync(dishId);
        return Ok(dish);
    }

    [HttpPost("{dishId}/review")]
    [RoleAuthorize(RoleType.Client)]
    public async Task<IActionResult> AddDishReview(Guid dishId, ReviewDTO reviewModel)
    {
        await _dishService.AddReviewToDishAsync(dishId, UserId, reviewModel);
        return Ok();
    }

    [HttpPost]
    [RoleAuthorize(RoleType.Manager)]
    public async Task<IActionResult> CreateDish(DishCreation dishModel, [FromQuery] Guid restaurantId)
    {
        await _permissionService.RestaurantOwnerValidate(UserId, restaurantId);
        await _dishService.CreateDishAsync(dishModel, restaurantId);
        return Ok();
    }

    [HttpPut("{dishId}")]
    [RoleAuthorize(RoleType.Manager)]
    public async Task<ActionResult<DishDTO>> UpdateDish(Guid dishId, DishCreation dishModel)
    {
        await _permissionService.DishOwnerValidate(UserId, dishId);
        var dish = await _dishService.UpdateDishAsync(dishModel, dishId);
        return Ok(dish);
    }

    [HttpDelete("{dishId}")]
    [RoleAuthorize(RoleType.Manager)]
    public async Task<IActionResult> DeleteDish(Guid dishId)
    {
        await _permissionService.DishOwnerValidate(UserId, dishId);
        await _dishService.DeleteDishAsync(dishId);
        return Ok();
    }
}
