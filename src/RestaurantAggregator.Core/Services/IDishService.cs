using RestaurantAggregator.Core.Data;
using RestaurantAggregator.Core.Data.DTO;

namespace RestaurantAggregator.Core.Services;

public interface IDishService
{
    Task<DishDTO> GetDishInfoByIdAsync(Guid id);
    Task CreateDishAsync(DishCreation dish);
    Task UpdateDishAsync(DishCreation dish, Guid id);
    Task DeleteDishAsync(Guid id);
    Task AddReviewToDishAsync(Guid dishId, Guid clientId, ReviewDTO reviewModel);
}
