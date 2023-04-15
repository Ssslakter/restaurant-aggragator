using RestaurantAggregator.Core.Data.DTO;

namespace RestaurantAggregator.Core.Services;

public interface ICartService
{
    Task AddDishToCartAsync(Guid dishId, Guid clientId, uint quantity);
    Task RemoveDishFromCartAsync(Guid dishId, Guid clientId, uint quantity);
    Task<CartDTO> GetCartAsync(Guid clientId);
    Task ClearCartAsync(Guid clientId);
}
