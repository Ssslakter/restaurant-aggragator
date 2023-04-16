using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantAggregator.Core.Data.DTO;
using RestaurantAggregator.Core.Data.Enums;
using RestaurantAggregator.Core.Services;
using RestaurantAggregator.Infra.Auth;

namespace RestaurantAggregator.Api.Controllers;

[ApiController]
[RoleAuthorize(RoleType.Client)]
[Route("cart")]
public class CartController : AuthControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    [HttpGet]
    public async Task<ActionResult<CartDTO>> GetCart()
    {
        return Ok(await _cartService.GetCartAsync(UserId));
    }

    [HttpPost("{dishId}/add")]
    public async Task<IActionResult> AddDishToCart(Guid dishId, [FromQuery] uint quantity = 1)
    {
        await _cartService.AddDishToCartAsync(dishId, UserId, quantity);
        return Ok();
    }

    [HttpPost("{dishId}/remove")]
    public async Task<IActionResult> RemoveDishFromCart(Guid dishId, [FromQuery] uint quantity = 1)
    {
        await _cartService.RemoveDishFromCartAsync(dishId, UserId, quantity);
        return Ok();
    }

    [HttpPost("clear")]
    public async Task<IActionResult> ClearCart()
    {
        await _cartService.ClearCartAsync(UserId);
        return Ok();
    }
}
