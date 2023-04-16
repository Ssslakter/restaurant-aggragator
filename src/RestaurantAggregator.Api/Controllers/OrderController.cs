using Microsoft.AspNetCore.Mvc;
using RestaurantAggregator.Core.Data.Enums;
using RestaurantAggregator.Core.Data.DTO;
using RestaurantAggregator.Infra.Auth;
using RestaurantAggregator.Core.Services;

namespace RestaurantAggregator.Api.Controllers;

[ApiController]
[Route("orders")]
public class OrderController : AuthControllerBase
{
    private readonly IOrderService _orderService;
    private readonly IPermissionService _permissionService;

    public OrderController(IOrderService orderService, IPermissionService permissionService)
    {
        _orderService = orderService;
        _permissionService = permissionService;
    }

    [RoleAuthorize(RoleType.Client)]
    [HttpPost("create")]
    public async Task<IActionResult> CreateOrderFromCart(DeliveryAddress address)
    {
        var fullAddress = $"{address.City}, {address.Street}, {address.House}";
        await _orderService.CreateOrderFromCartAsync(UserId, fullAddress);
        return Ok();
    }

    [RoleAuthorize(RoleType.Client, RoleType.Courier, RoleType.Cook)]
    [HttpGet("{orderId}/info")]
    public async Task<ActionResult<OrderDetails>> GetOrderInfo(Guid orderId)
    {
        if (!await _permissionService.OrderParticipant(UserId, orderId, roleType: null))
            return Forbid("You are not allowed to view this order");
        var order = await _orderService.GetOrderByIdAsync(orderId);
        return Ok(order);
    }

    [RoleAuthorize(RoleType.Client)]
    [HttpGet("history")]
    public async Task<ActionResult<ICollection<OrderDTO>>> GetPersonalOrderHistory([FromQuery] bool deliveredOnly,
     [FromQuery] uint page = 1)
    {
        OrderStatus? status = deliveredOnly ? OrderStatus.Delivered : null;
        var orders = await _orderService.GetOrdersByClientIdAsync(UserId, status, page);
        return Ok(orders);
    }

    [HttpGet("kitchen/{restaurantId}")]
    [RoleAuthorize(RoleType.Manager, RoleType.Cook)]
    public async Task<ActionResult<ICollection<OrderDTO>>> GetOrdersForCook(Guid restaurantId,
        [FromQuery] bool createdOnly, [FromQuery] uint page)
    {
        if (!createdOnly)
        {
            return Ok(await _orderService.GetOrdersByCookIdAsync(UserId, page));
        }
        if (!await _permissionService.RestaurantStaff(UserId, restaurantId))
            return Forbid("You are not allowed to view this restaurant orders");
        return Ok(await _orderService.GetOrdersByRestaurantIdAsync(restaurantId, OrderStatus.Created, page));
    }

    [HttpPatch("kitchen/{orderId}/status")]
    [RoleAuthorize(RoleType.Cook)]
    public async Task<IActionResult> ChangeOrderStatusKitchen(Guid orderId, OrderStatus status)
    {
        if (status == OrderStatus.Canceled || !await _permissionService.CanChangeOrderStatusUp(UserId, orderId))
            return Forbid("You are not allowed to change this order status");
        await _orderService.ChangeOrderStatusAsync(orderId, status);
        if (status == OrderStatus.Kitchen)
        {
            await _orderService.AssingCookToOrderAsync(orderId, UserId);
        }
        return Ok();
    }

    [HttpPatch("delivery/{orderId}/status")]
    [RoleAuthorize(RoleType.Cook)]
    public async Task<IActionResult> ChangeOrderStatusDelivery(Guid orderId, OrderStatus status)
    {
        if (!await _permissionService.CanChangeOrderStatusUp(UserId, orderId))
            return Forbid("You are not allowed to change this order status");
        await _orderService.ChangeOrderStatusAsync(orderId, status);
        if (status == OrderStatus.Delivery)
        {
            await _orderService.AssingCourierToOrderAsync(orderId, UserId);
        }
        return Ok();
    }

    [HttpGet("courier")]
    [RoleAuthorize(RoleType.Courier)]
    public async Task<ActionResult<ICollection<OrderDTO>>> GetOrdersForCourier([FromQuery] uint page, [FromQuery] bool packaging)
    {
        if (packaging)
            return Ok(await _orderService.GetOrdersByStatusAsync(OrderStatus.Packaging, page));
        return Ok(await _orderService.GetOrdersByCourierIdAsync(UserId, page));
    }

    [RoleAuthorize(RoleType.Client)]
    [HttpPost("{orderId}/cancel")]
    public async Task<IActionResult> CancelOrder(Guid orderId)
    {
        if (!await _permissionService.OrderParticipant(UserId, orderId, RoleType.Client))
            return Forbid("You are not allowed to cancel this order");
        await _orderService.ChangeOrderStatusAsync(orderId, OrderStatus.Canceled);
        return Ok();
    }
}
