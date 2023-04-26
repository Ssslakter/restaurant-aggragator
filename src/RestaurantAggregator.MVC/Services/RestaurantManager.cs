using RestaurantAggregator.MVC.Services.Client;
using RestaurantCreation = RestaurantAggregator.MVC.Services.Client.RestaurantCreation;
using RestaurantDTO = RestaurantAggregator.MVC.Services.Client.RestaurantDTO;

namespace RestaurantAggregator.MVC.Services;

public class RestaurantManager : IRestaurantManager
{
    private readonly IRestaurantApiClient _restaurantApiClient;

    public RestaurantManager(IRestaurantApiClient restaurantApiClient)
    {
        _restaurantApiClient = restaurantApiClient;
        //TODO _restaurantApiClient.SetToken(smthn);
    }
    public async Task<RestaurantDTO> CreateRestaurantAsync(RestaurantCreation restaurant)
    {
        return await _restaurantApiClient.RestaurantPOSTAsync(restaurant);
    }

    public async Task DeleteRestaurantAsync(Guid restaurantId)
    {
        await _restaurantApiClient.RestaurantDELETEAsync(restaurantId);
    }

    public async Task<IEnumerable<ProfileDTO>> GetRestaurantStaffAsync(Guid restaurantId)
    {
        return await _restaurantApiClient.StaffAsync(restaurantId);
    }

    public async Task<IEnumerable<RestaurantDTO>> GetRestaurantsAsync(int page)
    {
        return await _restaurantApiClient.RestaurantAllAsync(page);
    }

    public async Task<RestaurantDTO> UpdateRestaurantAsync(Guid restaurantId, RestaurantCreation restaurant)
    {
        return await _restaurantApiClient.RestaurantPUTAsync(restaurantId, restaurant);
    }
}
