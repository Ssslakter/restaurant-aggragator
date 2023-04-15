using Microsoft.AspNetCore.Mvc;
using RestaurantAggregator.Core.Data.DTO;

namespace RestaurantAggregator.Api.Controllers;

[ApiController]
[Route("restaurant")]
public class RestaurantController : ControllerBase
{
    [HttpGet("all")]
    public Task<ActionResult<ICollection<RestaurantDTO>>> GetAllRestaurants()
    {
        throw new NotImplementedException();
    }

    [HttpGet("{name}")]
    public Task<ActionResult<ICollection<RestaurantDTO>>> GetRestaurantsByName(string name)
    {
        throw new NotImplementedException();
    }
}
