using Microsoft.AspNetCore.Mvc;

namespace RestaurantAggregator.Api.Controllers;

[ApiController]
[Route("orders")]
public class OrderController : ControllerBase
{
    [HttpPost("create")]
    public Task<IActionResult> CreateOrderFromCart(string orderModel)
    {
        throw new NotImplementedException();
    }

    [HttpGet("{orderId}/info")]
    public Task<IActionResult> GetOrderInfo(Guid orderId)
    {
        throw new NotImplementedException();
    }

    [HttpGet("history")]
    public Task<IActionResult> GetOrderHistory([FromQuery] uint page)
    {
        throw new NotImplementedException();
    }

    [HttpPost("{orderId}/cancel")]
    public Task<IActionResult> CancelOrder(Guid orderId)
    {
        throw new NotImplementedException();
    }

    [HttpPatch("{orderId}/status")]
    public Task<IActionResult> ChangeOrderStatus(Guid orderId, string status)
    {
        throw new NotImplementedException();
    }

    [HttpGet("{orderId}/status")]
    public Task<IActionResult> GetOrderStatus(Guid orderId)
    {
        throw new NotImplementedException();
    }

    [HttpGet]
    public Task<IActionResult> GetOrders([FromQuery] string filters,
        [FromQuery] string sorting,
        [FromQuery] uint page)
    {
        throw new NotImplementedException();
    }
}
