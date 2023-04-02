using RestaurantAggregator.Core.Data;
using RestaurantAggregator.Core.Data.DTO;

namespace RestaurantAggregator.Core.Services;

public interface ICartService
{
    Task AddDishToCartAsync(Guid dishId, Guid clientId);
    Task RemoveDishFromCartAsync(Guid dishId, Guid clientId);
    Task<CartDTO> GetCartAsync(Guid clientId);
    Task ClearCartAsync(Guid clientId);
}
