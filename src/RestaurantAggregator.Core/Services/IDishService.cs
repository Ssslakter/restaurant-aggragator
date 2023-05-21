using RestaurantAggregator.Core.Data.DTO;

namespace RestaurantAggregator.Core.Services;

public interface IDishService
{
    Task<IEnumerable<DishDTO>> GetDishesAsync(Guid restaurantId);
    Task<DishDTO> GetDishInfoByIdAsync(Guid dishId, Guid restaurantId);
    Task<DishDTO> CreateDishAsync(DishCreation dish, Guid restaurantId);
    Task<DishDTO> UpdateDishAsync(DishCreation dish, Guid dishId, Guid restaurantId);
    Task DeleteDishAsync(Guid dishId, Guid restaurantId);
    Task AddReviewToDishAsync(Guid dishId, Guid clientId, ReviewDTO reviewModel);
}
