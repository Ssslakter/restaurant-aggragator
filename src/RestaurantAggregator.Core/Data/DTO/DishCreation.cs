using RestaurantAggregator.Core.Data.Enums;

namespace RestaurantAggregator.Core.Data.DTO;
#nullable disable

public class DishCreation
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public bool IsVegeterian { get; set; }
    public string Photo { get; set; }
    public Category Category { get; set; }
}
