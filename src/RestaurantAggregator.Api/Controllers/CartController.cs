using Microsoft.AspNetCore.Mvc;
using RestaurantAggregator.Core.Data.DTO;
using RestaurantAggregator.Core.Data.Enums;
using RestaurantAggregator.Core.Services;
using RestaurantAggregator.Infra.Auth;

namespace RestaurantAggregator.Api.Controllers;

[ApiController]
[RoleAuthorize(RoleType.Client)]
[Route("api/cart")]
public class CartController : AuthControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    /// <summary>
    /// Shows cart
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<CartDTO>> GetCart()
    {
        return Ok(await _cartService.GetCartAsync(UserId));
    }

    /// <summary>
    /// Adds dish to cart
    /// </summary>
    [HttpPost("{dishId}")]
    public async Task<IActionResult> AddDishToCart(Guid dishId, [FromQuery] uint quantity = 1)
    {
        await _cartService.AddDishToCartAsync(dishId, UserId, quantity);
        return Ok();
    }

    /// <summary>
    /// Removes dish from cart
    /// </summary>
    [HttpDelete("{dishId}")]
    public async Task<IActionResult> RemoveDishFromCart(Guid dishId, [FromQuery] uint quantity = 1)
    {
        await _cartService.RemoveDishFromCartAsync(dishId, UserId, quantity);
        return Ok();
    }

    /// <summary>
    /// Clears cart
    /// </summary>
    [HttpDelete]
    public async Task<IActionResult> ClearCart()
    {
        await _cartService.ClearCartAsync(UserId);
        return Ok();
    }
}
