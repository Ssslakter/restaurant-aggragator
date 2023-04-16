using Microsoft.AspNetCore.Mvc;
using RestaurantAggregator.Core.Data.DTO;
using RestaurantAggregator.Core.Services;

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
}
