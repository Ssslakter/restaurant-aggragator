using Microsoft.AspNetCore.Mvc;
using RestaurantAggregator.Core.Data.Enums;
using RestaurantAggregator.Core.Data.DTO;
using RestaurantAggregator.Infra.Auth;
using RestaurantAggregator.Core.Services;
using RestaurantAggregator.Core.Exceptions;

namespace RestaurantAggregator.Api.Controllers;

[ApiController]
[Route("api")]
public class OrderController : AuthControllerBase
{
    private readonly IOrderService _orderService;
    private readonly IPermissionService _permissionService;

    public OrderController(IOrderService orderService, IPermissionService permissionService)
    {
        _orderService = orderService;
        _permissionService = permissionService;
    }

    /// <summary>
    /// Creates order from cart
    /// </summary>
    [RoleAuthorize(RoleType.Client)]
    [HttpPost("orders/cart")]
    public async Task<IActionResult> CreateOrderFromCart(DeliveryAddress address)
    {
        var fullAddress = $"{address.City}, {address.Street}, {address.House}";
        await _orderService.CreateOrderFromCartAsync(UserId, fullAddress);
        return Ok();
    }

    /// <summary>
    /// Gets order info by its id
    /// </summary>
    [RoleAuthorize(RoleType.Client, RoleType.Courier, RoleType.Cook)]
    [HttpGet("orders/{orderId}")]
    public async Task<ActionResult<OrderDetails>> GetOrderInfo(Guid orderId)
    {
        try
        {
            await _permissionService.OrderParticipantValidate(UserId, orderId, roleType: null);
        }
        catch (ForbidException)
        {
            await _permissionService.CanChangeOrderStatusUpValidate(UserId, orderId, UserRoles);
        }
        var order = await _orderService.GetOrderByIdAsync(orderId);
        return Ok(order);
    }

    /// <summary>
    /// Gets order history for client
    /// </summary>
    [RoleAuthorize(RoleType.Client)]
    [HttpGet("orders")]
    public async Task<ActionResult<ICollection<OrderDTO>>> GetPersonalOrderHistory([FromQuery] bool deliveredOnly,
     [FromQuery] uint page = 1)
    {
        OrderStatus? status = deliveredOnly ? OrderStatus.Delivered : null;
        var orders = await _orderService.GetOrdersByClientIdAsync(UserId, status, page);
        return Ok(orders);
    }

    /// <summary>
    /// Gets orders for restaurant staff
    /// </summary>
    [HttpGet("restaurant/{restaurantId}/kitchen/orders")]
    [RoleAuthorize(RoleType.Cook)]
    public async Task<ActionResult<ICollection<OrderWithDishes>>> GetOrdersForCook(Guid restaurantId,
        [FromQuery] OrderStatus status, [FromQuery] uint page)
    {
        await _permissionService.RestaurantCookValidate(UserId, restaurantId);
        if (status == OrderStatus.Created)
        {
            return Ok(await _orderService.GetOrdersByRestaurantIdAsync(restaurantId, status, page));
        }
        return Ok(await _orderService.GetOrdersByCookIdAsync(UserId, page));
    }

    /// <summary>
    /// Change order status for cook
    /// </summary>
    [HttpPatch("restaurant/{restaurantId}/kitchen/orders/{orderId}")]
    [RoleAuthorize(RoleType.Cook)]
    public async Task<IActionResult> ChangeOrderStatusKitchen(Guid orderId, OrderStatus status)
    {
        if (status == OrderStatus.Canceled)
            throw new ForbidException("You can't cancel order in kitchen");
        await _permissionService.CanChangeOrderStatusUpValidate(UserId, orderId, UserRoles);
        if (status == OrderStatus.Kitchen)
        {
            await _orderService.AssingCookToOrderAsync(orderId, UserId);
        }
        await _orderService.ChangeOrderStatusAsync(orderId, status);
        return Ok();
    }

    /// <summary>
    /// Change order status for courier
    /// </summary>
    [HttpPatch("delivery/orders/{orderId}")]
    [RoleAuthorize(RoleType.Courier)]
    public async Task<IActionResult> ChangeOrderStatusDelivery(Guid orderId, OrderStatus status)
    {
        await _permissionService.CanChangeOrderStatusUpValidate(UserId, orderId, new[] { RoleType.Courier });
        if (status == OrderStatus.Delivery)
        {
            await _orderService.AssingCourierToOrderAsync(orderId, UserId);
        }
        await _orderService.ChangeOrderStatusAsync(orderId, status);
        return Ok();
    }

    /// <summary>
    /// Gets orders for courier
    /// </summary>
    [HttpGet("delivery/orders")]
    [RoleAuthorize(RoleType.Courier)]
    public async Task<ActionResult<ICollection<OrderDTO>>> GetOrdersForCourier([FromQuery] uint page, [FromQuery] bool packaging)
    {
        if (packaging)
            return Ok(await _orderService.GetOrdersByStatusAsync(OrderStatus.Packaging, page));
        return Ok(await _orderService.GetOrdersByCourierIdAsync(UserId, page));
    }

    /// <summary>
    /// Cancel order by client
    /// </summary>
    /// <remarks>
    /// Only client can cancel order using this method<br/>
    /// For other roles use patch method
    /// </remarks>
    [RoleAuthorize(RoleType.Client)]
    [HttpPost("orders/{orderId}/cancel")]
    public async Task<IActionResult> CancelOrder(Guid orderId)
    {
        await _permissionService.OrderParticipantValidate(UserId, orderId, RoleType.Client);
        await _orderService.ChangeOrderStatusAsync(orderId, OrderStatus.Canceled);
        return Ok();
    }
}
