using Microsoft.AspNetCore.Mvc;

namespace RestaurantAggregator.Auth.Controllers;

[ApiController]
[Route("api/profile")]
public class ProfileController : ControllerBase
{
    [HttpGet]
    public IActionResult GetProfile()
    {
        return Ok();
    }

    [HttpPut]
    public IActionResult UpdateProfile()
    {
        return Ok();
    }
}
