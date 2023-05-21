using RestaurantAggregator.Core.Data.Enums;

namespace RestaurantAggregator.DAL.Data;
#nullable disable
public class Dish
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public bool IsVegeterian { get; set; }
    public string Photo { get; set; }
    public Category Category { get; set; }
    public Guid RestaurantId { get; set; }
    public Restaurant Restaurant { get; set; }
    public ICollection<Review> Reviews { get; set; }
}
