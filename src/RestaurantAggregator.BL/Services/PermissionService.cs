using RestaurantAggregator.Core.Data.Enums;
using RestaurantAggregator.Core.Exceptions;
using RestaurantAggregator.Core.Services;
using RestaurantAggregator.DAL;

namespace RestaurantAggregator.BL.Services;

public class PermissionService : IPermissionService
{
    private readonly RestaurantDbContext _context;

    public PermissionService(RestaurantDbContext context)
    {
        _context = context;
    }

    public async Task<bool> RestaurantOwner(Guid userId, Guid restaurantId)
    {
        var restaurant = await _context.Restaurants.FindAsync(restaurantId);
        if (restaurant == null)
            throw new NotFoundInDbException("Restaurant not found");

        return restaurant.Managers.Any(x => x == userId);
    }

    public async Task<bool> RestaurantStaff(Guid userId, Guid restaurantId)
    {
        var restaurant = await _context.Restaurants.FindAsync(restaurantId);
        if (restaurant == null)
            throw new NotFoundInDbException("Restaurant not found");
        return restaurant.Managers.Any(x => x == userId) || restaurant.Cooks.Any(x => x == userId);
    }

    public async Task<bool> OrderParticipant(Guid userId, Guid orderId, RoleType? roleType)
    {
        var order = await _context.Orders.FindAsync(orderId);
        if (order == null)
            throw new NotFoundInDbException("Order not found");
        if (roleType == null)
            return order.ClientId == userId || order.CourierId == userId || order.CookId == userId;
        return (order.ClientId == userId && roleType == RoleType.Client) ||
            (order.CourierId == userId && roleType == RoleType.Courier) ||
            (order.CookId == userId && roleType == RoleType.Cook);
    }

    public async Task<bool> CanChangeOrderStatusUp(Guid userId, Guid orderId)
    {
        var order = await _context.Orders.FindAsync(orderId);
        if (order == null)
            throw new NotFoundInDbException("Order not found");

        return order.Status switch
        {
            OrderStatus.Created => order.CookId == userId && await RestaurantStaff(userId, order.RestaurantId),
            OrderStatus.Kitchen => order.CookId == userId && await OrderParticipant(userId, orderId, RoleType.Cook),
            OrderStatus.Packaging => order.CourierId == userId,
            OrderStatus.Delivery => order.CourierId == userId && await OrderParticipant(userId, orderId, RoleType.Courier),
            OrderStatus.Delivered => false,
            OrderStatus.Canceled => false,
            _ => false
        };
    }
}