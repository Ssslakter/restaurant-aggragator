namespace RestaurantAggregator.Core.Data.DTO;
#nullable disable
public class OrderDetails : OrderDTO
{
    public List<DishInOrderDTO> Dishes { get; set; }
    public string ClientName { get; set; }
    public string CourierName { get; set; }
}
