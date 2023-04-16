using RestaurantAggregator.Core.Data.Enums;
using RestaurantAggregator.Core.Data.DTO;

namespace RestaurantAggregator.Core.Services;

public interface IOrderService
{
    Task CreateOrderFromCartAsync(Guid clientId, string address);
    Task<OrderDetails> GetOrderByIdAsync(Guid id);
    Task<ICollection<OrderDTO>> GetOrdersByClientIdAsync(Guid clientId, OrderStatus? status, uint page);
    Task<ICollection<OrderDTO>> GetOrdersByCourierIdAsync(Guid courierId, uint page);
    Task<ICollection<OrderDTO>> GetOrdersByCookIdAsync(Guid cookId, uint page);
    Task<ICollection<OrderDTO>> GetOrdersByRestaurantIdAsync(Guid restaurantId, OrderStatus? status, uint page);
    Task<ICollection<OrderDTO>> GetOrdersByStatusAsync(OrderStatus status, uint page);
    Task ChangeOrderStatusAsync(Guid orderId, OrderStatus status);
    Task AssingCourierToOrderAsync(Guid orderId, Guid courierId);
    Task AssingCookToOrderAsync(Guid orderId, Guid cookId);
}
