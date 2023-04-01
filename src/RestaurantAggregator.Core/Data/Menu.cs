namespace RestaurantAggregator.Core.Data;
#nullable disable

public class Menu
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public ICollection<Dish> Dishes { get; set; }
    public Guid RestaurantId { get; set; }
}
