using RestaurantAggregator.Core.Data.DTO;

namespace RestaurantAggregator.Core.Services;

public interface IRestaurantService
{
    Task<ICollection<RestaurantDTO>> GetRestaurantsAsync(uint page);
    Task<ICollection<RestaurantDTO>> GetRestaurantsByNameAsync(string name, uint page);
    Task<RestaurantDTO> CreateRestaurantAsync(RestaurantCreation restaurantModel);
    Task<RestaurantDTO> UpdateRestaurantAsync(Guid id, RestaurantCreation restaurantModel);
    Task DeleteRestaurantAsync(Guid id);
    Task<IEnumerable<ProfileWithRolesDTO>> GetRestaurantStaffAsync(Guid restaurantId);
}
