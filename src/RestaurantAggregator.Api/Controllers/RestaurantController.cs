using Microsoft.AspNetCore.Mvc;
using RestaurantAggregator.Core.Data.DTO;
using RestaurantAggregator.Core.Data.Enums;
using RestaurantAggregator.Core.Services;
using RestaurantAggregator.Infra.Auth;

namespace RestaurantAggregator.Api.Controllers;

[ApiController]
[Route("restaurant")]
public class RestaurantController : ControllerBase
{
    private readonly IRestaurantService _restaurantService;

    public RestaurantController(IRestaurantService restaurantService)
    {
        _restaurantService = restaurantService;
    }

    [HttpGet("all")]
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
}
