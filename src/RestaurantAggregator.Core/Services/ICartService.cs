using RestaurantAggregator.Core.Data;

namespace RestaurantAggregator.Core.Services;

public interface ICartService
{
    Task AddDishToCartAsync(Guid dishId, Guid clientId);
    Task RemoveDishFromCartAsync(Guid dishId, Guid clientId);
    Task<Cart> GetCartAsync(Guid clientId);
    Task ClearCartAsync(Guid clientId);
}
