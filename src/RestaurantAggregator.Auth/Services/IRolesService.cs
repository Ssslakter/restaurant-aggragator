using RestaurantAggregator.Core.Data.Enums;

namespace RestaurantAggregator.Auth.Services;

public interface IRolesService
{
    Task AddRoleToUserAsync(Guid userId, RoleType roleType);
    Task RemoveRoleFromUserAsync(Guid userId, RoleType roleType);
    Task<IEnumerable<RoleType>> GetUserRolesAsync(Guid userId);
}