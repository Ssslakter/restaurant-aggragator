using RestaurantAggregator.Auth.Client.Services;
using RestaurantAggregator.MVC.Services.Client;
using ProfileDTO = RestaurantAggregator.Auth.Client.Services.ProfileDTO;

namespace RestaurantAggregator.MVC.Services;

public class UserManager : IUserManager
{
    private readonly IUserService _usersService;
    private readonly IRestaurantApiClient _restaurantApiClient;

    public UserManager(IRestaurantApiClient restaurantApiClient)
    {
        _usersService = null;
        _restaurantApiClient = restaurantApiClient;
    }

    public async Task<IEnumerable<ProfileDTO>> GetUsersAsync(int page)
    {
        return await _usersService.GetUsersAsync(page);
    }
}
