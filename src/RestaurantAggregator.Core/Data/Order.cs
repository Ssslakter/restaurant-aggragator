using RestaurantAggregator.Core.Data.Enums;

namespace RestaurantAggregator.Core.Data;
#nullable disable

public class Order
{
    public Guid Id { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime OrderTime { get; set; }
    public DateTime? DeliveryTime { get; set; }
    public string Address { get; set; }
    public double Price { get; set; }
    public List<Dish> Dishes { get; set; }
    //Client Id
    public Guid UserId { get; set; }
}