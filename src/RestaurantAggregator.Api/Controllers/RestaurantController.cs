using Microsoft.AspNetCore.Mvc;

namespace RestaurantAggregator.Api.Controllers;

[ApiController]
[Route("restaurant")]
public class RestaurantController : ControllerBase
{
    [HttpGet("all")]
    public Task<IActionResult> GetAllRestaurants()
    {
        throw new NotImplementedException();
    }
}
