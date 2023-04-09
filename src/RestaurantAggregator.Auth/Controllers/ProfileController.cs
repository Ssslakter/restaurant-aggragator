using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RestaurantAggregator.Auth.Controllers;

[ApiController]
[Route("api/profile")]
public class ProfileController : ControllerBase
{
    [HttpGet]
    [Authorize]
    public IActionResult GetProfile()
    {
        return Ok();
    }

    [HttpPut]
    [Authorize]
    public IActionResult UpdateProfile()
    {
        return Ok();
    }
}
