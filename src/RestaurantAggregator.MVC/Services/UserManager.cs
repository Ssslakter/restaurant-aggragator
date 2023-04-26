using RestaurantAggregator.Auth.Client.Services;
using ProfileDTO = RestaurantAggregator.Auth.Client.Services.ProfileDTO;

namespace RestaurantAggregator.MVC.Services;

public class UserManager : IUserManager
{
    public Task AddRoleToUserAsync(Guid userId, RoleType roleType)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ProfileDTO>> GetUserProfilesAsync(uint page)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<RoleType>> GetUserRolesAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task RemoveRoleFromUserAsync(Guid userId, RoleType roleType)
    {
        throw new NotImplementedException();
    }
}
