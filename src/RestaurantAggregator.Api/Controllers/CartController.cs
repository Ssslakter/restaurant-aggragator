using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantAggregator.Core.Data.DTO;
using RestaurantAggregator.Core.Services;
using RestaurantAggregator.Infra.Utils;

namespace RestaurantAggregator.Api.Controllers;

[ApiController]
//TODO: add auth
[Route("cart")]
public class CartController : AuthControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    [HttpGet]
    [Authorize(Roles = "Client")]
    public async Task<ActionResult<CartDTO>> GetCart()
    {
        return Ok(await _cartService.GetCartAsync(UserId));
    }

    [HttpPost("{dishId}/add")]
    public async Task<IActionResult> AddDishToCart(Guid dishId)
    {
        throw new NotImplementedException();
    }

    [HttpPost("{dishId}/remove")]
    public async Task<IActionResult> RemoveDishFromCart(Guid dishId)
    {
        throw new NotImplementedException();
    }

    [HttpPost("clear")]
    public async Task<IActionResult> ClearCart()
    {
        throw new NotImplementedException();
    }
}
