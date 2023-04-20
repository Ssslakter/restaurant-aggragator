using RestaurantAggregator.Core.Data.DTO;

namespace RestaurantAggregator.Core.Services;

public interface IDishService
{
    Task<DishDTO> GetDishInfoByIdAsync(Guid id);
    Task<DishDTO> CreateDishAsync(DishCreation dish, Guid restaurantId);
    Task<DishDTO> UpdateDishAsync(DishCreation dish, Guid id);
    Task DeleteDishAsync(Guid id);
    Task AddReviewToDishAsync(Guid dishId, Guid clientId, ReviewDTO reviewModel);
}
