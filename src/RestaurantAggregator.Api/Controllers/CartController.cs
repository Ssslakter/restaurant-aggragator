using Microsoft.AspNetCore.Mvc;
using RestaurantAggregator.Core.Data.DTO;

namespace RestaurantAggregator.Api.Controllers;

[ApiController]
//TODO: add auth
[Route("cart")]
public class CartController : ControllerBase
{
    [HttpGet]
    public Task<ActionResult<CartDTO>> GetCart()
    {
        throw new NotImplementedException();
    }

    [HttpPost("{dishId}/add")]
    public Task<IActionResult> AddDishToCart(Guid dishId)
    {
        throw new NotImplementedException();
    }

    [HttpPost("{dishId}/remove")]
    public Task<IActionResult> RemoveDishFromCart(Guid dishId)
    {
        throw new NotImplementedException();
    }

    [HttpPost("clear")]
    public Task<IActionResult> ClearCart()
    {
        throw new NotImplementedException();
    }
}
