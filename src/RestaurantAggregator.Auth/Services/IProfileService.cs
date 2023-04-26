using RestaurantAggregator.Core.Data.DTO;

namespace RestaurantAggregator.Auth.Services;

public interface IProfileService
{
    Task UpdateProfileAsync(Guid userId, ProfileCreation profileCreation);
    Task<ProfileDTO> GetProfileAsync(Guid userId);
    Task<IEnumerable<ProfileDTO>> GetUserProfilesAsync(uint page = 1);
    Task DeleteUserAsync(Guid userId);
}