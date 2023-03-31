using Microsoft.AspNetCore.Mvc;

namespace RestaurantAggregator.Api.Controllers;

[ApiController]
[Route("cart")]
public class CartController : ControllerBase
{
    [HttpGet]
    public Task<IActionResult> GetCart()
    {
        throw new NotImplementedException();
    }

    [HttpPost("{dishId}/add")]
    public Task<IActionResult> AddDishToCart(Guid dishId, uint quantity)
    {
        throw new NotImplementedException();
    }

    [HttpPost("{dishId}/remove")]
    public Task<IActionResult> RemoveDishFromCart(Guid dishId, uint quantity)
    {
        throw new NotImplementedException();
    }

    [HttpPost("clear")]
    public Task<IActionResult> ClearCart()
    {
        throw new NotImplementedException();
    }
}
