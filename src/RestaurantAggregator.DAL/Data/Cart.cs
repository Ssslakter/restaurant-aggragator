namespace RestaurantAggregator.DAL.Data;
#nullable disable

public class Cart
{
    public Guid Id { get; set; }
    public ICollection<DishInCart> Dishes { get; set; }
}

public class DishInCart
{
    public Guid Id { get; set; }
    public Cart Cart { get; set; }
    public Dish Dish { get; set; }
    public uint Count { get; set; }
}