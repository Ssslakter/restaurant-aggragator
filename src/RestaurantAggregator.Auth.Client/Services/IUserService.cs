using RoleTypeCore = RestaurantAggregator.Core.Data.Enums.RoleType;
namespace RestaurantAggregator.Auth.Client.Services;

public interface IUserService
{
    Task<ProfileDTO> GetProfileAsync();
    Task<ProfileDTO> GetProfileAsync(Guid userId);
    Task<IEnumerable<RoleTypeCore>> GetUserRolesAsync(Guid userId);
    Task<IEnumerable<ProfileDTO>> GetUsersAsync(int page);
}
