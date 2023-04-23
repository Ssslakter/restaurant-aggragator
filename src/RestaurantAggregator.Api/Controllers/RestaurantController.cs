using Microsoft.AspNetCore.Mvc;
using RestaurantAggregator.Core.Data.DTO;
using RestaurantAggregator.Core.Data.Enums;
using RestaurantAggregator.Core.Services;
using RestaurantAggregator.Infra.Auth;

namespace RestaurantAggregator.Api.Controllers;

[ApiController]
[Route("api/restaurant")]
public class RestaurantController : AuthControllerBase
{
    private readonly IRestaurantService _restaurantService;
    private readonly IPermissionService _permissionService;

    public RestaurantController(IRestaurantService restaurantService, IPermissionService permissionService)
    {
        _restaurantService = restaurantService;
        _permissionService = permissionService;
    }

    [HttpGet]
    public async Task<ActionResult<ICollection<RestaurantDTO>>> GetAllRestaurants(uint page = 1)
    {
        return Ok(await _restaurantService.GetRestaurantsAsync(page));
    }

    [HttpGet("{name}")]
    public async Task<ActionResult<ICollection<RestaurantDTO>>> GetRestaurantsByName(string name, uint page = 1)
    {
        return Ok(await _restaurantService.GetRestaurantsByNameAsync(name, page));
    }

    [HttpPost]
    [RoleAuthorize(RoleType.Admin)]
    public async Task<ActionResult<RestaurantDTO>> CreateRestaurant(RestaurantCreation restaurantModel)
    {
        var restaurant = await _restaurantService.CreateRestaurantAsync(restaurantModel);
        return Ok(restaurant);
    }

    [HttpPut("{id}")]
    [RoleAuthorize(RoleType.Admin)]
    public async Task<ActionResult<RestaurantDTO>> UpdateRestaurant(Guid id, RestaurantCreation restaurantModel)
    {
        var restaurant = await _restaurantService.UpdateRestaurantAsync(id, restaurantModel);
        return Ok(restaurant);
    }

    [HttpDelete("{id}")]
    [RoleAuthorize(RoleType.Admin)]
    public async Task<IActionResult> DeleteRestaurant(Guid id)
    {
        await _restaurantService.DeleteRestaurantAsync(id);
        return Ok();
    }

    [HttpPost("{id}/managers/{userId}")]
    [RoleAuthorize(RoleType.Admin)]
    public async Task<IActionResult> GiveManagerRole(Guid id, Guid userId)
    {
        await _permissionService.GivePermissionToUser(userId, id, RoleType.Manager);
        return Ok();
    }

    [HttpDelete("{id}/managers/{userId}")]
    [RoleAuthorize(RoleType.Admin)]
    public async Task<IActionResult> RevokeManagerRole(Guid id, Guid userId)
    {
        await _permissionService.RevokePermissionFromUser(userId, id, RoleType.Manager);
        return Ok();
    }

    [HttpPost("{id}/cooks/{userId}")]
    [RoleAuthorize(RoleType.Admin, RoleType.Manager)]
    public async Task<IActionResult> GiveCookRole(Guid id, Guid userId)
    {
        if (!UserRoles.Contains(RoleType.Admin))
        {
            await _permissionService.RestaurantOwnerValidate(UserId, id);
        }
        await _permissionService.GivePermissionToUser(userId, id, RoleType.Cook);
        return Ok();
    }

    [HttpDelete("{id}/cooks/{userId}")]
    [RoleAuthorize(RoleType.Admin, RoleType.Manager)]
    public async Task<IActionResult> RevokeCookRole(Guid id, Guid userId)
    {
        if (!UserRoles.Contains(RoleType.Admin))
        {
            await _permissionService.RestaurantOwnerValidate(UserId, id);
        }
        await _permissionService.RevokePermissionFromUser(userId, id, RoleType.Cook);
        return Ok();
    }
}
