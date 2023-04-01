using RestaurantAggregator.Core.Data;

namespace RestaurantAggregator.Api.Data.DTO;
#nullable disable
public class OrderDetails : OrderDTO
{
    public List<Dish> Dishes { get; set; }
}
