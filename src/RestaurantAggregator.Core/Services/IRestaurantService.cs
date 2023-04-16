using RestaurantAggregator.Core.Data.DTO;

namespace RestaurantAggregator.Core.Services;

public interface IRestaurantService
{
    Task<ICollection<RestaurantDTO>> GetRestaurantsAsync(uint page);
    Task<ICollection<RestaurantDTO>> GetRestaurantsByNameAsync(string name, uint page);
}
