namespace RestaurantAggregator.Core.Data;
#nullable disable

public class Restaurant
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public ICollection<Menu> Menus { get; set; }
}
