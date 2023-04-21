using RestaurantAggregator.Core.Data.Enums;

namespace RestaurantAggregator.Core.Services;

public interface IPermissionService
{
    Task RestaurantStaffValidate(Guid userId, Guid restaurantId);
    Task RestaurantOwnerValidate(Guid userId, Guid restaurantId);
    Task OrderParticipantValidate(Guid userId, Guid orderId, RoleType? roleType);
    Task CanChangeOrderStatusUpValidate(Guid userId, Guid orderId);
}
