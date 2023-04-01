using RestaurantAggregator.Core.Data.Enums;

namespace RestaurantAggregator.Core.Data;
#nullable disable
public class Dish
{
    public string Name { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public bool IsVegeterian { get; set; }
    public string Photo { get; set; }
    public Category Category { get; set; }
}
