using RestaurantAggregator.Core.Data.Enums;

namespace RestaurantAggregator.Core.Data.DTO;

public class OrderStatusNotification
{
    public Guid ClientId { get; set; }
    public int OrderNumber { get; set; }
    public OrderStatus Status { get; set; }
}
