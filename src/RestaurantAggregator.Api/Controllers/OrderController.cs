using Microsoft.AspNetCore.Mvc;
using RestaurantAggregator.Api.Data.DTO;
using RestaurantAggregator.Core.Data.Enums;

namespace RestaurantAggregator.Api.Controllers;

[ApiController]
[Route("orders")]
public class OrderController : ControllerBase
{
    //Auth user
    [HttpPost("create")]
    public Task<IActionResult> CreateOrderFromCart()
    {
        throw new NotImplementedException();
    }

    //Auth user
    [HttpGet("{orderId}/info")]
    public Task<ActionResult<OrderDTO>> GetOrderInfo(Guid orderId)
    {
        throw new NotImplementedException();
    }

    //Auth user
    [HttpGet("history")]
    public Task<ActionResult<ICollection<OrderDTO>>> GetPersonalOrderHistory([FromQuery] uint page)
    {
        throw new NotImplementedException();
    }

    //Auth user
    [HttpGet("current")]
    public Task<ActionResult<ICollection<OrderDTO>>> GetPersonalCurrentOrders([FromQuery] uint page)
    {
        throw new NotImplementedException();
    }

    [HttpGet("history/{restaurantId}")]
    public Task<ActionResult<ICollection<OrderDTO>>> GetRestaurantOrderHistory(Guid restaurantId,
        [FromQuery] uint page)
    {
        throw new NotImplementedException();
    }

    [HttpGet("current/{restaurantId}")]
    public Task<ActionResult<ICollection<OrderDTO>>> GetRestaurantCurrentOrders(Guid restaurantId, [FromQuery] uint page)
    {
        throw new NotImplementedException();
    }

    //Auth user|Courier
    [HttpPost("{orderId}/cancel")]
    public Task<IActionResult> CancelOrder(Guid orderId)
    {
        throw new NotImplementedException();
    }

    //Auth Cook|Courier
    [HttpPatch("{orderId}/status")]
    public Task<IActionResult> ChangeOrderStatus(Guid orderId, OrderStatus status)
    {
        throw new NotImplementedException();
    }

    //Auth user
    [HttpGet("{orderId}/status")]
    public Task<ActionResult<OrderStatus>> GetOrderStatus(Guid orderId)
    {
        throw new NotImplementedException();
    }
}
