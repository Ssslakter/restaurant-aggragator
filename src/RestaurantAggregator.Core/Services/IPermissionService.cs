using RestaurantAggregator.Core.Data.Enums;

namespace RestaurantAggregator.Core.Services;

public interface IPermissionService
{
    Task<bool> RestaurantStaff(Guid userId, Guid restaurantId);
    Task<bool> RestaurantOwner(Guid userId, Guid restaurantId);
    Task<bool> OrderParticipant(Guid userId, Guid orderId, RoleType? roleType);
    Task<bool> CanChangeOrderStatusUp(Guid userId, Guid orderId);
}
