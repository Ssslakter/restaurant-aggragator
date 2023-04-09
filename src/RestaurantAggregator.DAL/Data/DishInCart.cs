namespace RestaurantAggregator.DAL.Data;
#nullable disable

public class DishInCart
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    public Guid DishId { get; set; }
    public Dish Dish { get; set; }
    public uint Count { get; set; }
}