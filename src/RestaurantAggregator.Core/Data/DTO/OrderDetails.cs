using RestaurantAggregator.Core.Data.DTO;

namespace RestaurantAggregator.Api.Data.DTO;
#nullable disable
public class OrderDetails : OrderDTO
{
    public List<DishDTO> Dishes { get; set; }
}
