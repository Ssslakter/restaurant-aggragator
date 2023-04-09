using RestaurantAggregator.Api.Data.DTO;
using RestaurantAggregator.Core.Data;
using RestaurantAggregator.Core.Data.Enums;

namespace RestaurantAggregator.Core.Services;

public interface IOrderService
{
    Task CreateOrderFromCartAsync(Guid clientId);
    Task<OrderDetails> GetOrderByIdAsync(Guid id);
    Task<ICollection<OrderDTO>> GetOrdersByClientIdAsync(Guid clientId, OrderStatus? status);
    Task<ICollection<OrderDTO>> GetOrdersByRestaurantIdAsync(Guid restaurantId, OrderStatus? status);
    Task ChangeOrderStatusAsync(Guid orderId, OrderStatus status);
    Task<OrderStatus> GetOrderStatusAsync(Guid orderId);
}
