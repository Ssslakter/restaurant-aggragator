using RestaurantAggregator.Api.Data.DTO;
using RestaurantAggregator.Core.Data.DTO;
using RestaurantAggregator.Core.Data.Enums;

namespace RestaurantAggregator.Core.Services;

public interface IOrderService
{
    Task CreateOrderFromCartAsync(CartDTO cart, Guid userId);
    Task<OrderDetails> GetOrderByIdAsync(Guid id);
    Task<ICollection<OrderDTO>> GetOrdersByUserIdAsync(Guid userId, OrderStatus? status);
    Task<ICollection<OrderDTO>> GetOrdersByRestaurantIdAsync(Guid restaurantId, OrderStatus? status);
    Task ChangeOrderStatusAsync(Guid orderId, OrderStatus status);
    Task<OrderStatus> GetOrderStatusAsync(Guid orderId);
}
