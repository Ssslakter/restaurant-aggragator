using RestaurantAggregator.Auth.Client.Services;

namespace RestaurantAggregator.MVC.Services;

public interface IUserManager
{
    Task AddRoleToUserAsync(Guid userId, RoleType roleType);
    Task RemoveRoleFromUserAsync(Guid userId, RoleType roleType);
    Task<IEnumerable<RoleType>> GetUserRolesAsync(Guid userId);
    Task<IEnumerable<ProfileDTO>> GetUserProfilesAsync(uint page);
}
