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

    [HttpPut("{restaurantId}")]
    [RoleAuthorize(RoleType.Admin)]
    public async Task<ActionResult<RestaurantDTO>> UpdateRestaurant(Guid restaurantId, RestaurantCreation restaurantModel)
    {
        var restaurant = await _restaurantService.UpdateRestaurantAsync(restaurantId, restaurantModel);
        return Ok(restaurant);
    }

    [HttpDelete("{restaurantId}")]
    [RoleAuthorize(RoleType.Admin)]
    public async Task<IActionResult> DeleteRestaurant(Guid restaurantId)
    {
        await _restaurantService.DeleteRestaurantAsync(restaurantId);
        return Ok();
    }

    [HttpPost("{restaurantId}/managers/{userId}")]
    [RoleAuthorize(RoleType.Admin)]
    public async Task<IActionResult> GiveManagerRole(Guid restaurantId, Guid userId)
    {
        await _permissionService.GivePermissionToUser(userId, restaurantId, RoleType.Manager);
        return Ok();
    }

    [HttpDelete("{restaurantId}/managers/{userId}")]
    [RoleAuthorize(RoleType.Admin)]
    public async Task<IActionResult> RevokeManagerRole(Guid restaurantId, Guid userId)
    {
        await _permissionService.RevokePermissionFromUser(userId, restaurantId, RoleType.Manager);
        return Ok();
    }

    [HttpPost("{restaurantId}/cooks/{userId}")]
    [RoleAuthorize(RoleType.Admin, RoleType.Manager)]
    public async Task<IActionResult> GiveCookRole(Guid restaurantId, Guid userId)
    {
        if (!UserRoles.Contains(RoleType.Admin))
        {
            await _permissionService.RestaurantOwnerValidate(UserId, restaurantId);
        }
        await _permissionService.GivePermissionToUser(userId, restaurantId, RoleType.Cook);
        return Ok();
    }

    [HttpDelete("{restaurantId}/cooks/{userId}")]
    [RoleAuthorize(RoleType.Admin, RoleType.Manager)]
    public async Task<IActionResult> RevokeCookRole(Guid restaurantId, Guid userId)
    {
        if (!UserRoles.Contains(RoleType.Admin))
        {
            await _permissionService.RestaurantOwnerValidate(UserId, restaurantId);
        }
        await _permissionService.RevokePermissionFromUser(userId, restaurantId, RoleType.Cook);
        return Ok();
    }

    [HttpGet("{restaurantId}/staff")]
    [RoleAuthorize(RoleType.Admin, RoleType.Manager)]
    public async Task<ActionResult<ICollection<ProfileDTO>>> GetRestaurantStaff(Guid restaurantId)
    {
        if (!UserRoles.Contains(RoleType.Admin))
        {
            await _permissionService.RestaurantOwnerValidate(UserId, restaurantId);
        }
        return Ok(await _restaurantService.GetRestaurantStaffAsync(restaurantId));
    }
}
