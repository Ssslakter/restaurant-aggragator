using RestaurantAggregator.Core.Data;
using RestaurantAggregator.Core.Data.Enums;
using RestaurantAggregator.Core.Data.DTO;

namespace RestaurantAggregator.Core.Services;

public interface IOrderService
{
    Task CreateOrderFromCartAsync(Guid clientId, string address);
    Task<OrderDetails> GetOrderByIdAsync(Guid id);
    Task<ICollection<OrderDTO>> GetOrdersByClientIdAsync(Guid clientId, OrderStatus? status);
    Task<ICollection<OrderDTO>> GetOrdersByCourierIdAsync(Guid courierId, OrderStatus? status);
    Task<ICollection<OrderDTO>> GetOrdersByCookIdAsync(Guid cookId, OrderStatus? status);
    Task<ICollection<OrderDTO>> GetOrdersByRestaurantIdAsync(Guid restaurantId, OrderStatus? status);
    Task ChangeOrderStatusAsync(Guid orderId, OrderStatus status);
    Task<OrderStatus> GetOrderStatusAsync(Guid orderId);
    Task AssingCourierToOrderAsync(Guid orderId, Guid courierId);
    Task AssingCookToOrderAsync(Guid orderId, Guid cookId);
}
