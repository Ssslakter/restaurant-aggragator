using RestaurantAggregator.Auth.Client.Services;
using RestaurantAggregator.Core.Data.Enums;
using RestaurantAggregator.Core.Exceptions;
using RestaurantAggregator.Core.Services;
using RestaurantAggregator.DAL;
using RoleType = RestaurantAggregator.Core.Data.Enums.RoleType;

namespace RestaurantAggregator.BL.Services;

public class PermissionService : IPermissionService
{
    private readonly RestaurantDbContext _context;
    private readonly IUserService _userService;

    public PermissionService(RestaurantDbContext context, IUserService userService)
    {
        _context = context;
        _userService = userService;
    }

    public async Task RestaurantOwnerValidate(Guid userId, Guid restaurantId)
    {
        if (!await RestaurantOwner(userId, restaurantId))
        {
            throw new ForbidException("You are not a manager of this restaurant");
        }
    }

    public async Task RestaurantCookValidate(Guid userId, Guid restaurantId)
    {
        if (!await RestaurantCook(userId, restaurantId))
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

    public async Task CanChangeOrderStatusUpValidate(Guid userId, Guid orderId, IEnumerable<RoleType> roleTypes)
    {
        var order = await _context.Orders.FindAsync(orderId);
        if (order == null)
            throw new NotFoundInDbException($"Order with id {orderId} not found");

        var flag = order.Status switch
        {
            OrderStatus.Created => (await RestaurantCook(userId, order.RestaurantId) && roleTypes.Contains(RoleType.Cook))
             || (await RestaurantOwner(userId, order.RestaurantId) && roleTypes.Contains(RoleType.Manager)),
            OrderStatus.Kitchen => order.CookId == userId
             && await OrderParticipant(userId, orderId, RoleType.Cook),
            OrderStatus.Packaging => roleTypes.Contains(RoleType.Courier),
            OrderStatus.Delivery => order.CourierId == userId
             && await OrderParticipant(userId, orderId, RoleType.Courier),
            OrderStatus.Delivered => false,
            OrderStatus.Canceled => false,
            _ => false
        };
        if (!flag)
        {
            throw new ForbidException("You can't change order status");
        }
    }

    private async Task<bool> RestaurantCook(Guid userId, Guid restaurantId)
    {
        var restaurant = await _context.Restaurants.FindAsync(restaurantId);
        if (restaurant == null)
            throw new NotFoundInDbException($"Restaurant with id {restaurantId} not found");
        return restaurant.Cooks.Any(x => x == userId);
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

    public async Task GivePermissionToUser(Guid userId, Guid restaurantId,
     RoleType roleType)
    {
        var restaurant = await _context.Restaurants.FindAsync(restaurantId);
        if (restaurant == null)
            throw new NotFoundInDbException($"Restaurant with id {restaurantId} not found");
        var userRoleTypes = await _userService.GetUserRolesAsync(userId);
        switch (roleType)
        {
            case RoleType.Cook:
                if (!userRoleTypes.Contains(RoleType.Cook))
                {
                    throw new BusinessException("User cannot be a cook of this restaurant " +
                    "because he doesn't have Cook role");
                }

                if (restaurant.Cooks.Any(x => x == userId))
                    throw new DbViolationException("User already has this permission");

                restaurant.Cooks.Add(userId);
                break;
            case RoleType.Manager:
                if (!userRoleTypes.Contains(RoleType.Manager))
                {
                    throw new BusinessException("User cannot be a manager of this restaurant " +
                    "because he doesn't have Manager role");
                }
                if (restaurant.Managers.Any(x => x == userId))
                    throw new DbViolationException("User already has this permission");

                restaurant.Managers.Add(userId);
                break;
            default:
                throw new BusinessException("RoleType must be Cook or Manager");
        }
        await _context.SaveChangesAsync();
    }

    public async Task RevokePermissionFromUser(Guid userId, Guid restaurantId, RoleType roleType)
    {
        var restaurant = await _context.Restaurants.FindAsync(restaurantId);
        if (restaurant == null)
            throw new NotFoundInDbException($"Restaurant with id {restaurantId} not found");
        switch (roleType)
        {
            case RoleType.Cook:
                if (!restaurant.Cooks.Any(x => x == userId))
                    throw new DbViolationException("User doesn't have this permission");
                restaurant.Cooks.Remove(userId);
                break;
            case RoleType.Manager:
                if (!restaurant.Managers.Any(x => x == userId))
                    throw new DbViolationException("User doesn't have this permission");
                restaurant.Managers.Remove(userId);
                break;
            default:
                throw new BusinessException("RoleType must be Cook or Manager");
        }
        await _context.SaveChangesAsync();
    }
}