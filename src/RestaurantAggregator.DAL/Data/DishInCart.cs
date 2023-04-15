namespace RestaurantAggregator.DAL.Data;
#nullable disable

public class DishInCart
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    public Guid DishId { get; set; }
    public Dish Dish { get; set; }
    public uint Count { get; set; }
    /// <summary>
    /// The price of the dish at the time of creating the order
    /// </summary>
    public decimal Price { get; set; }
    /// <summary>
    /// True if the order with this dish was created from the cart<br/>
    /// False if the order is in the cart
    /// </summary>
    public bool InOrder { get; set; }
    public Guid? OrderId { get; set; }
    public Order Order { get; set; }
}