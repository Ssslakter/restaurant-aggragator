using System.ComponentModel.DataAnnotations;
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
    public Task<ActionResult<OrderDTO>> CreateOrderFromCart(string orderModel)
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
    public Task<ActionResult<ICollection<OrderDTO>>> GetOrderHistory([FromQuery] uint page)
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

    [HttpGet]
    public Task<IActionResult> GetOrders([FromQuery, EnumDataType(typeof(Category))] ICollection<string> filters,
        [FromQuery] string sorting,
        [FromQuery] uint page)
    {
        throw new NotImplementedException();
    }
}
