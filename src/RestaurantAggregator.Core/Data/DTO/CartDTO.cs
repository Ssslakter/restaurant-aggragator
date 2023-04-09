namespace RestaurantAggregator.Core.Data.DTO;
#nullable disable
public class CartDTO
{
    public Guid ClientId { get; set; }
    public ICollection<DishInCartDTO> Dishes { get; set; }
}

public class DishInCartDTO
{
    public DishDTO Dish { get; set; }
    public uint Count { get; set; }
}