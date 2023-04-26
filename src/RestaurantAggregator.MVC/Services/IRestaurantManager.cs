using RestaurantAggregator.MVC.Services.Client;

namespace RestaurantAggregator.MVC.Services;

public interface IRestaurantManager
{
    Task<IEnumerable<RestaurantDTO>> GetRestaurantsAsync(int page);
    Task<IEnumerable<ProfileDTO>> GetRestaurantStaffAsync(Guid restaurantId);
    Task<RestaurantDTO> CreateRestaurantAsync(RestaurantCreation restaurant);
    Task<RestaurantDTO> UpdateRestaurantAsync(Guid restaurantId, RestaurantCreation restaurant);
    Task DeleteRestaurantAsync(Guid restaurantId);
}
