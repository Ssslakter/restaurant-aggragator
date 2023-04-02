using RestaurantAggregator.Core.Data;
using RestaurantAggregator.Core.Data.DTO;
using RestaurantAggregator.Core.Services;

namespace RestaurantAggregator.BL;

public class DishService : IDishService
{
    public Task AddReviewToDishAsync(Guid dishId, Guid clientId, ReviewDTO reviewModel)
    {
        throw new NotImplementedException();
    }

    public Task CreateDishAsync(DishCreation dish)
    {
        throw new NotImplementedException();
    }

    public Task DeleteDishAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<DishDTO> GetDishInfoByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task UpdateDishAsync(DishCreation dish, Guid id)
    {
        throw new NotImplementedException();
    }
}
