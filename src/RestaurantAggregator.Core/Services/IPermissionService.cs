using RestaurantAggregator.Core.Data.Enums;

namespace RestaurantAggregator.Core.Services;

public interface IPermissionService
{
    Task RestaurantCookValidate(Guid userId, Guid restaurantId);
    Task RestaurantOwnerValidate(Guid userId, Guid restaurantId);
    Task OrderParticipantValidate(Guid userId, Guid orderId, RoleType? roleType);
    Task CanChangeOrderStatusUpValidate(Guid userId, Guid orderId, IEnumerable<RoleType> roleTypes);
    Task GivePermissionToUser(Guid userId, Guid restaurantId, RoleType roleType);
    Task RevokePermissionFromUser(Guid userId, Guid restaurantId, RoleType roleType);
}
