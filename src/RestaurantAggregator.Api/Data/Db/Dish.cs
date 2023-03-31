namespace RestaurantAggregator.Api.Data.Db;

public class Dish
{
    public String Name { get; set; }
    public String Description { get; set; }
    public double Price { get; set; }
    public bool IsVegeterian { get; set; }
    public String Photo { get; set; }
}
