namespace RestaurantAggregator.Auth.Client.Services;

public interface IUserService
{
    Task<ProfileDTO> GetProfile();
    Task<ProfileDTO> GetProfile(Guid userId);
}
