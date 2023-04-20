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

    public async Task RestaurantOwnerValidate(Guid userId, Guid restaurantId)
    {
        if (!await RestaurantOwner(userId, restaurantId))
        {
            throw new ForbidException("You are not a manager of this restaurant");
        }
    }

    public async Task RestaurantStaffValidate(Guid userId, Guid restaurantId)
    {
        if (!await RestaurantStaff(userId, restaurantId))
        {
            throw new ForbidException("You are not a staff of this restaurant");
        }
    }

    public async Task OrderParticipantValidate(Guid userId, Guid orderId, RoleType? roleType)
    {
        if (!await OrderParticipant(userId, orderId, roleType))
        {
            throw new ForbidException("You are not a participant of this order with correct role");
        }
    }

    public async Task CanChangeOrderStatusUpValidate(Guid userId, Guid orderId)
    {
        var order = await _context.Orders.FindAsync(orderId);
        if (order == null)
            throw new NotFoundInDbException($"Order with id {orderId} not found");

        var flag = order.Status switch
        {
            OrderStatus.Created => await RestaurantStaff(userId, order.RestaurantId),
            OrderStatus.Kitchen => order.CookId == userId && await OrderParticipant(userId, orderId, RoleType.Cook),
            OrderStatus.Packaging => order.CourierId == userId,
            OrderStatus.Delivery => order.CourierId == userId && await OrderParticipant(userId, orderId, RoleType.Courier),
            OrderStatus.Delivered => false,
            OrderStatus.Canceled => false,
            _ => false
        };
        if (!flag)
        {
            throw new ForbidException("You can't change order status");
        }
    }

    public async Task DishOwnerValidate(Guid userId, Guid dishId)
    {
        var dish = await _context.Dishes.FindAsync(dishId);
        if (dish == null)
            throw new NotFoundInDbException($"Dish with id {dishId} not found");
        var restaurantId = dish.RestaurantId;
        if (!await RestaurantOwner(userId, restaurantId))
        {
            throw new ForbidException("You are not allowed to modify the dish of this restaurant");
        }
    }

    public async Task MenuOwnerValidate(Guid userId, Guid menuId)
    {
        var menu = await _context.Menus.FindAsync(menuId);
        if (menu == null)
            throw new NotFoundInDbException($"Menu with id {menuId} not found");
        var restaurantId = menu.RestaurantId;
        if (!await RestaurantOwner(userId, restaurantId))
        {
            throw new ForbidException("You are not allowed to modify the menu of this restaurant");
        }
    }

    private async Task<bool> RestaurantStaff(Guid userId, Guid restaurantId)
    {
        var restaurant = await _context.Restaurants.FindAsync(restaurantId);
        if (restaurant == null)
            throw new NotFoundInDbException($"Restaurant with id {restaurantId} not found");
        return restaurant.Managers.Any(x => x == userId) || restaurant.Cooks.Any(x => x == userId);
    }

    private async Task<bool> RestaurantOwner(Guid userId, Guid restaurantId)
    {
        var restaurant = await _context.Restaurants.FindAsync(restaurantId);
        if (restaurant == null)
            throw new NotFoundInDbException($"Restaurant with id {restaurantId} not found");
        return restaurant.Managers.Any(x => x == userId);
    }

    private async Task<bool> OrderParticipant(Guid userId, Guid orderId, RoleType? roleType)
    {
        var order = await _context.Orders.FindAsync(orderId);
        if (order == null)
            throw new NotFoundInDbException($"Order with id {orderId} not found");
        if (roleType == null)
            return order.ClientId == userId || order.CourierId == userId || order.CookId == userId;
        return (order.ClientId == userId && roleType == RoleType.Client) ||
            (order.CourierId == userId && roleType == RoleType.Courier) ||
            (order.CookId == userId && roleType == RoleType.Cook);
    }
}