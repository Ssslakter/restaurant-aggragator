namespace RestaurantAggregator.DAL.Data;
#nullable disable
public class Client
{
    public Guid Id { get; set; }
    public Cart Cart { get; set; }
    public ICollection<Order> OrderHistory { get; set; }
}
