using RestaurantAggregator.Core.Data.DTO;

namespace RestaurantAggregator.Core.Services;

public interface IDishService
{
    Task<DishDTO> GetDishInfoByIdAsync(Guid id);
    Task CreateDishAsync(DishCreation dish, Guid menuId);
    Task UpdateDishAsync(DishCreation dish, Guid id);
    Task DeleteDishAsync(Guid id);
    Task AddReviewToDishAsync(Guid dishId, Guid clientId, ReviewDTO reviewModel);
    Task<double> GetDishRatingAsync(Guid dishId);
}
